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
    public class SettingLaborDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public SettingLaborDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<SettingLaborDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<SettingLaborDetailsImportRowDto>();
            var rows = new List<SettingLaborDetailsImportRowDto>();

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
                        var settingVendor = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var settingType = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var shapeCode = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var shape = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var wtFromStr = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var wtToStr = worksheet.Cell(row, 7).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(settingVendor)
                            && string.IsNullOrWhiteSpace(settingType)
                            && string.IsNullOrWhiteSpace(shapeCode)
                            && string.IsNullOrWhiteSpace(shape)
                            && string.IsNullOrWhiteSpace(wtFromStr)
                            && string.IsNullOrWhiteSpace(wtToStr))
                            continue;

                        rows.Add(new SettingLaborDetailsImportRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            SettingVendor = settingVendor,
                            SettingType = settingType,
                            ShapeCode = shapeCode,
                            Shape = shape,
                            DiamondPSWtFrom = decimal.TryParse(wtFromStr, out var wtFrom)
                                                ? wtFrom : (decimal?)null,
                            DiamondPSWtTo = decimal.TryParse(wtToStr, out var wtTo)
                                                ? wtTo : (decimal?)null,
                            GoldCostPS = worksheet.Cell(row, 8).GetValue<decimal>(),
                            PlatinumCostPS = worksheet.Cell(row, 9).GetValue<decimal>(),
                            SilverCostPS = worksheet.Cell(row, 10).GetValue<decimal>()
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
                else if (row.SettingVendor.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "setting vendor cannot exceed 50 characters.";
                }
                else if (row.ShapeCode.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "shape code cannot exceed 10 characters.";
                }
                else if (row.Shape.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "shape cannot exceed 50 characters.";
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
                _repository.GetAllSettingLaborDetailsCodes()
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

                    row.ErrorMessage3 = "Code already exists in DB-- will be replce";
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

        public int ImportSettingLaborDetails(List<SettingLaborDetailsImportRowDto> rows)
        {
            var codesToDelete = rows
               .Where(x => x.IsExistingInDb)
               .Select(x => x.Code)
               .ToList();

            if (codesToDelete.Any())
            {
                _repository.DeleteSettingLaborDetailsByCodes(codesToDelete);
            }
            var records = rows.Select(x => new SettingLaborDetail
            {
                Code = x.Code,
                SettingVendor = x.SettingVendor,
                SettingType = x.SettingType,
                ShapeCode = x.ShapeCode,
                Shape = x.Shape,
                DiamondPSWtFrom = x.DiamondPSWtFrom,
                DiamondPSWtTo = x.DiamondPSWtTo,
                GoldCostPS = x.GoldCostPS,
                PlatinumCostPS = x.PlatinumCostPS,
                SilverCostPS = x.SilverCostPS
            }).ToList();

            _repository.BulkInsertSettingLaborDetails(records);
            return records.Count;
        }
    }
}