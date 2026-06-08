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

                    // STEP 0 — VALIDATE COLUMN HEADERS
                    // Must be first check before reading any rows
                    // =============================================
                    var expectedHeaders = new List<string>
                    {
                         "CompanyCode", "StoneVendorCode", "StoneType",
                        "StoneShapeCode", "StoneShape", "StoneQualityCode","StoneQuality", "InternationalGrading"
                    };
                                        for (int i = 0; i < expectedHeaders.Count; i++)
                    {
                        var actualHeader = worksheet.Cell(1, i + 1).GetValue<string>().Trim();

                        if (!actualHeader.Equals(expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                        {
                            result.IsValidTemplate = false;
                            result.TemplateError = $"Invalid Excel template. " +
                                $"Expected column '{expectedHeaders[i]}' at position {i + 1} " +
                                $"but found '{(string.IsNullOrWhiteSpace(actualHeader) ? "empty" : actualHeader)}'.";
                            return result; // ← stop immediately, no further processing
                        }
                    }
                    //

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var companyCode = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var stoneVendorCode = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var stoneType = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var stoneShapeCode = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var stoneShape = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var stoneQualityCode = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var stoneQuality = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var internationalGrading = worksheet.Cell(row, 8).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if ( string.IsNullOrWhiteSpace(companyCode)
                            && string.IsNullOrWhiteSpace(stoneVendorCode)
                            && string.IsNullOrWhiteSpace(stoneType)
                            && string.IsNullOrWhiteSpace(stoneShapeCode)
                            && string.IsNullOrWhiteSpace(stoneShape)
                            && string.IsNullOrWhiteSpace(stoneQualityCode)
                            && string.IsNullOrWhiteSpace(stoneQuality)
                            && string.IsNullOrWhiteSpace(internationalGrading))
                            continue;

                        rows.Add(new StoneQualityDetailsImportRowDto
                        {
                            RowNumber = row,
                            CompanyCode = companyCode,
                            StoneVendorCode = stoneVendorCode,
                            StoneType = stoneType,
                            StoneShapeCode = stoneShapeCode,
                            StoneShape = stoneShape,
                            StoneQualityCode = stoneQualityCode,
                            StoneQuality = stoneQuality,
                            InternationalGrading = internationalGrading
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // VALIDATION
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.StoneQualityCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code is required.";
                }
                else if (row.StoneQualityCode.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code cannot exceed 10 characters.";
                }
                else if (row.StoneShapeCode.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "stone shape code cannot exceed 10 characters.";
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
                .GroupBy(x => x.StoneQualityCode.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (codesInExcel.Contains(row.StoneQualityCode.ToLower()))
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
                if (row.IsDuplicate) continue;
                if (codesInDb.Contains(row.StoneQualityCode.ToLower()))
                {
                    row.IsExistingInDb = true;
                    row.ErrorMessage3 = "Code already exists in DB - will be replaced";
                }
                else
                {
                    // Not in DB  new record
                    row.IsNew = true; // ← NEW
                }
            }

            result.ValidRecords = rows.Where(x => x.IsValid && !x.IsDuplicate).ToList();
            result.InvalidRecords = rows.Where(x => !x.IsValid).ToList();
            result.DuplicateRecords = rows.Where(x => x.IsDuplicate).ToList();
            result.ExistingInDbRecords = rows.Where(x => x.IsExistingInDb).ToList();
            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;
            result.ExistingInDbRows = result.ExistingInDbRecords.Count;
            //result.NewRows = rows.Count(x => x.IsValid && !x.IsDuplicate && !x.IsExistingInDb);
            result.NewRecords = rows.Where(x => x.IsNew).ToList();
            result.NewRows = result.NewRecords.Count;

            return result;
        }

        public int ImportStoneQualityDetails(List<StoneQualityDetailsImportRowDto> rows)
        {
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.StoneQualityCode)
                .ToList();

            if (codesToDelete.Any())
            {
                // Delete old records from DB for these VendorCodes
                _repository.DeleteStoneQualityDetailsByCodes(codesToDelete);
            }

            var records = rows.Select(x => new StoneQualityDetailsDbDto
            {
                Company = x.CompanyCode,
                StoneVendorCode = x.StoneVendorCode,
                StoneType = x.StoneType,
                StoneShapeCode = x.StoneShapeCode,
                StoneShape = x.StoneShape,
                StoneQualityCode = x.StoneQualityCode,
                StoneQuality = x.StoneQuality,
                IntertionalGrading = x.InternationalGrading
            }).ToList();

            _repository.BulkInsertStoneQualityDetails(records);
            return records.Count;
        }
    }
}