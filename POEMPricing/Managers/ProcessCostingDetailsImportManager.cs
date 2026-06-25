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
    public class ProcessCostingDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public ProcessCostingDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<ProcessCostingDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<ProcessCostingDetailsImportRowDto>();
            var rows = new List<ProcessCostingDetailsImportRowDto>();

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
                        "Code", "VendorCode", "Category", "Type", "Unit",
                        "GoldCharges", "PlatinumCharges", "SilverCharges", "IsOptional"
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
                        var code = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var vendorCode = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var category = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var type = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var unit = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var goldChargesStr = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var platinumChargesStr = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var silverChargesStr = worksheet.Cell(row, 8).GetValue<string>().Trim();
                        var isOptionalStr = worksheet.Cell(row, 9).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(vendorCode)
                            && string.IsNullOrWhiteSpace(category)
                            && string.IsNullOrWhiteSpace(type)
                            && string.IsNullOrWhiteSpace(unit)
                            && string.IsNullOrWhiteSpace(goldChargesStr)
                            && string.IsNullOrWhiteSpace(platinumChargesStr)
                            && string.IsNullOrWhiteSpace(silverChargesStr))
                            continue;

                        rows.Add(new ProcessCostingDetailsImportRowDto
                        {
                            RowNumber = row,

                            // string values - if cell empty pass empty string
                            Code = code ?? "",
                            VendorCode = vendorCode ?? "",
                            Category = category ?? "",
                            Type = type ?? "",
                            Unit = unit ?? "",

                            // value types - if cell empty pass 0
                            GoldCharges = worksheet.Cell(row, 6).IsEmpty()
                                ? 0
                                : worksheet.Cell(row, 6).GetValue<decimal>(),

                            PlatinumCharges = worksheet.Cell(row, 7).IsEmpty()
                                ? 0
                                : worksheet.Cell(row, 7).GetValue<decimal>(),

                            SilverCharges = worksheet.Cell(row, 8).IsEmpty()
                                ? 0
                                : worksheet.Cell(row, 8).GetValue<decimal>(),

                            IsOptional = isOptionalStr.Equals("true", StringComparison.OrdinalIgnoreCase)
                    || isOptionalStr == "1"
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // Code is the unique key — required
            // =============================================
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
                else if (string.IsNullOrWhiteSpace(row.VendorCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Code is required.";
                }
                else if (string.IsNullOrWhiteSpace(row.Category))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category is required.";
                }
                else if (string.IsNullOrWhiteSpace(row.Type))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Type is required.";
                }
                else if (string.IsNullOrWhiteSpace(row.Unit))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Unit is required.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // Same Code more than once in sheet → REJECTED
            // =============================================
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

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK
            // Code exists in DB → IsExistingInDb = true
            // → stays in ValidRecords → REPLACE on confirm
            // =============================================
            var codesInDb = new HashSet<string>(
                _repository.GetAllProcessCostingDetailsCodes()
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
                    row.ErrorMessage3 = "Code already exists in DB — will be replaced.";
                }
                else
                {
                    // Not in DB  new record
                    row.IsNew = true; // ← NEW
                }
            }

            // =============================================
            // STEP 4 — SEPARATE INTO RESULT BUCKETS
            // =============================================
            result.ValidRecords = rows.Where(x => x.IsValid && !x.IsDuplicate).ToList();
            result.InvalidRecords = rows.Where(x => !x.IsValid).ToList();
            result.DuplicateRecords = rows.Where(x => x.IsDuplicate).ToList();
            result.ExistingInDbRecords = rows.Where(x => x.IsExistingInDb).ToList();
            result.NewRecords = rows.Where(x => x.IsNew).ToList();

            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;
            result.ExistingInDbRows = result.ExistingInDbRecords.Count;
            //result.NewRows = rows.Count(x => x.IsValid && !x.IsDuplicate && !x.IsExistingInDb);
            result.NewRows = result.NewRecords.Count;

            return result;
        }

        public int ImportProcessCostingDetails(List<ProcessCostingDetailsImportRowDto> rows)
        {
            // Step 1 — Delete existing DB records being replaced
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();


            // Step 2 — Insert all valid rows (new + replaced)
            var records = rows.Select(x => new ProcessCostingDetails
            {
                Code = x.Code,
                VendorCode = x.VendorCode,
                Category = x.Category,
                Type = x.Type,
                Unit = x.Unit,
                GoldCharges = x.GoldCharges,
                PlatinumCharges = x.PlatinumCharges,
                SilverCharges = x.SilverCharges,
                IsOptional = x.IsOptional
            }).ToList();

            _repository.ReplaceProcessCostingDetails(codesToDelete,records);
            return records.Count;
        }
    }
}