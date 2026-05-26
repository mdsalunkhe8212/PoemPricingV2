using ClosedXML.Excel;
using POEM.Model.Model;
using POEM.Model.Model.Import;
using POEM.Services.Interface;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace POEMPricing.Managers
{
    public class StoneQualityDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public StoneQualityDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<StoneQualityDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<StoneQualityDetailsImportRowDto>();
            var rows = new List<StoneQualityDetailsImportRowDto>();

            using (var stream = new MemoryStream())
            {
                file.InputStream.CopyTo(stream);

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rowCount = worksheet.LastRowUsed().RowNumber();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var code = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var companyCode = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var stoneVendorCode = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var stoneType = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var stoneShapeCode = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var stoneShape = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var stoneQualityCode = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var internationalGrading = worksheet.Cell(row, 8).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(companyCode)
                            && string.IsNullOrWhiteSpace(stoneVendorCode)
                            && string.IsNullOrWhiteSpace(stoneType)
                            && string.IsNullOrWhiteSpace(stoneShapeCode)
                            && string.IsNullOrWhiteSpace(stoneShape)
                            && string.IsNullOrWhiteSpace(stoneQualityCode)
                            && string.IsNullOrWhiteSpace(internationalGrading))
                            continue;

                        rows.Add(new StoneQualityDetailsImportRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            CompanyCode = companyCode,
                            StoneVendorCode = stoneVendorCode,
                            StoneType = stoneType,
                            StoneShapeCode = stoneShapeCode,
                            StoneShape = stoneShape,
                            StoneQualityCode = stoneQualityCode,
                            InternationalGrading = internationalGrading
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // VALIDATION
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.Code))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code is required.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // DUPLICATE INSIDE EXCEL - CODE
            var codesInExcel = new HashSet<string>(
                rows
                .Where(x => x.IsValid)
                .GroupBy(x => x.Code.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (codesInExcel.Contains(row.Code.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Code in Excel.";
                }
            }

            // DATABASE DUPLICATE CHECK
            var codesInDb = new HashSet<string>(
                _repository.GetAllStoneQualityDetailsCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (codesInDb.Contains(row.Code.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Code already exists in database.";
                }
            }

            result.ValidRecords = rows.Where(x => x.IsValid && !x.IsDuplicate).ToList();
            result.InvalidRecords = rows.Where(x => !x.IsValid).ToList();
            result.DuplicateRecords = rows.Where(x => x.IsDuplicate).ToList();
            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;

            return result;
        }

        public int ImportStoneQualityDetails(List<StoneQualityDetailsImportRowDto> rows)
        {
            var records = rows.Select(x => new StoneQualityDetailsDbDto
            {
                Code = x.Code,
                Company = x.CompanyCode,
                StoneVendorCode = x.StoneVendorCode,
                StoneType = x.StoneType,
                StoneShapeCode = x.StoneShapeCode,
                StoneShape = x.StoneShape,
                StoneQuality = x.StoneQualityCode,
                IntertionalGrading = x.InternationalGrading
            }).ToList();

            _repository.BulkInsertStoneQualityDetails(records);
            return records.Count;
        }
    }
}