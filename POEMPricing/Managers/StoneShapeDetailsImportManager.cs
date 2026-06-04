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
    public class StoneShapeDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public StoneShapeDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<StoneShapeDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<StoneShapeDetailsImportRowDto>();

            var rows = new List<StoneShapeDetailsImportRowDto>();

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
                        var stoneType = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var stoneShape = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var categoryFancyRound = worksheet.Cell(row, 4).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(stoneType)
                            && string.IsNullOrWhiteSpace(stoneShape)
                            && string.IsNullOrWhiteSpace(categoryFancyRound))
                            continue;

                        rows.Add(new StoneShapeDetailsImportRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            StoneType = stoneType,
                            StoneShape = stoneShape,
                            CategoryFancyRound = categoryFancyRound
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
                else if (row.Code.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code cannot exceed 10 characters.";
                }
                else if (row.StoneType.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "stone type cannot exceed 50 characters.";
                }
                else if (row.StoneShape.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "stone shape cannot exceed 50 characters.";
                }
                else if (row.CategoryFancyRound.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code cannot exceed 50 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // DUPLICATE INSIDE EXCEL - CODE

            var codesInExcel = new HashSet<string>(
                rows
                .Where(x => x.IsValid)  // ← ADD THIS
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
                _repository.GetAllStoneShapeDetailsCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );


            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (row.IsDuplicate) continue;
                if (codesInDb.Contains(row.Code.ToLower()))
                {
                    row.IsExistingInDb = true;

                    row.ErrorMessage3 = "Code already exists in DB- will be replace";
                }
                else
                {
                    // Not in DB  new record
                    row.IsNew = true; // ← NEW
                }

            }

            result.ValidRecords = rows
                .Where(x => x.IsValid && !x.IsDuplicate)
                .ToList();

            result.InvalidRecords = rows
                .Where(x => !x.IsValid)
                .ToList();

            result.DuplicateRecords = rows
                .Where(x => x.IsDuplicate)
                .ToList();

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

        public int ImportStoneShapeDetails(List<StoneShapeDetailsImportRowDto> rows)
        {
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();

            if (codesToDelete.Any())
            {
                // Delete old records from DB for these VendorCodes
                _repository.DeleteStoneShapeDetailsByCodes(codesToDelete);
            }
            var records = rows.Select(x => new StoneShapeDetail
            {
                Code = x.Code,
                StoneType = x.StoneType,
                StoneShape = x.StoneShape,
                CategoryFancyRound = x.CategoryFancyRound

            }).ToList();

            _repository.BulkInsertStoneShapeDetails(records);

            return records.Count;
        }

    }
}