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
    public class MarginDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public MarginDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<MarginDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<MarginDetailsImportRowDto>();
            var rows = new List<MarginDetailsImportRowDto>();

            using (var stream = new MemoryStream())
            {
                file.InputStream.CopyTo(stream);

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rowCount = worksheet.LastRowUsed().RowNumber();

                    // =============================================
                    // STEP 0 — VALIDATE COLUMN HEADERS
                    // Must be first check before reading any rows
                    // =============================================
                    var expectedHeaders = new List<string>
                    {
                        "Code", "Vendor", "CategoryCode", "Category",
                        "SubCategoryCode", "SubCategory", "Metal",
                        "PMargin1", "PMargin2", "PMargin3", "PMargin4"
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

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var code = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var vendor = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var categoryCode = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var category = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var subCategoryCode = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var subCategory = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var metal = worksheet.Cell(row, 7).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(vendor)
                            && string.IsNullOrWhiteSpace(categoryCode)
                            && string.IsNullOrWhiteSpace(category)
                            && string.IsNullOrWhiteSpace(subCategoryCode)
                            && string.IsNullOrWhiteSpace(subCategory)
                            && string.IsNullOrWhiteSpace(metal))
                            continue;

                        rows.Add(new MarginDetailsImportRowDto
                        {
                            RowNumber = row,

                            // string values - if cell empty pass empty string
                            Code = code ?? "",
                            Vendor = vendor ?? "",
                            CategoryCode = categoryCode ?? "",
                            Category = category ?? "",
                            SubCategoryCode = subCategoryCode ?? "",
                            SubCategory = subCategory ?? "",
                            Metal = metal ?? "",

                            // value types - if cell empty pass 0
                            PMargin1 = worksheet.Cell(row, 8).TryGetValue<decimal>(out var pm1)
                                ? pm1 : 0,

                            PMargin2 = worksheet.Cell(row, 9).TryGetValue<decimal>(out var pm2)
                                ? pm2 : 0,

                            PMargin3 = worksheet.Cell(row, 10).TryGetValue<decimal>(out var pm3)
                                ? pm3 : 0,

                            PMargin4 = worksheet.Cell(row, 11).TryGetValue<decimal>(out var pm4)
                                ? pm4 : 0
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // Code is the unique key — required + length checks
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
                else if (string.IsNullOrWhiteSpace(row.Vendor))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor is required.";
                }
                else if (row.Vendor.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.CategoryCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category Code is required.";
                }
                else if (row.CategoryCode.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category Code cannot exceed 10 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.Category))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category is required.";
                }
                else if (row.Category.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.SubCategoryCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sub Category Code is required.";
                }
                else if (row.SubCategoryCode.Length > 20)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sub Category Code cannot exceed 20 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.SubCategory))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sub Category is required.";
                }
                else if (row.SubCategory.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sub Category cannot exceed 50 characters.";
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
            var duplicateCodes = new HashSet<string>(
                rows
                .Where(x => x.IsValid)
                .GroupBy(x => x.Code.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;

                if (duplicateCodes.Contains(row.Code.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Code in Excel.";
                }
            }

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK
            // Code exists in DB → IsExistingInDb = true
            // → stays in ValidRecords → REPLACE on confirm
            // Else → IsNew = true → brand new record
            // =============================================
            var existingCodes = new HashSet<string>(
                _repository.GetAllMarginDetailsCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (row.IsDuplicate) continue;

                if (existingCodes.Contains(row.Code.ToLower()))
                {
                    row.IsExistingInDb = true;
                    row.ErrorMessage3 = "Code already exists in DB — will be replaced.";
                }
                else
                {
                    row.IsNew = true;
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
            result.NewRows = result.NewRecords.Count;

            return result;
        }

        public int ImportMarginDetails(List<MarginDetailsImportRowDto> rows)
        {
            // Step 1 — Delete existing DB records being replaced
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();

            if (codesToDelete.Any())
                _repository.DeleteMarginDetailsByCodes(codesToDelete);

            // Step 2 — Insert all valid rows (new + replaced)
            var records = rows.Select(x => new MarginDetailsDbDto
            {
                Code = x.Code,
                Vendor = x.Vendor,
                CategoryCode = x.CategoryCode,
                Category = x.Category,
                SubCategoryCode = x.SubCategoryCode,
                SubCategory = x.SubCategory,
                Metal = x.Metal,
                PMargin1 = x.PMargin1,
                PMargin2 = x.PMargin2,
                PMargin3 = x.PMargin3,
                PMargin4 = x.PMargin4
            }).ToList();

            _repository.BulkInsertMarginDetails(records);
            return records.Count;
        }
    }
}