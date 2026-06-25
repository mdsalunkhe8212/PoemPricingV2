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
    public class CategoryDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public CategoryDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<CategoryDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<CategoryDetailsImportRowDto>();
            var rows = new List<CategoryDetailsImportRowDto>();

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
                    var expectedHeaders = new List<string> { "Code", "CategoryName" };

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
                        var categoryName = worksheet.Cell(row, 2).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(categoryName))
                            continue;

                        rows.Add(new CategoryDetailsImportRowDto
                        {
                            RowNumber = row,

                            // string values - if cell empty pass empty string
                            Code = code ?? "",
                            CategoryName = categoryName ?? ""
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;





            // =============================================
            // STEP 1 — VALIDATION
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
                else if (row.CategoryName.Length > 100) 
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Category Name cannot exceed 100 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // Same Code or CategoryName more than once → REJECTED
            // =============================================
            var duplicateCodes = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.Code.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key));

            var duplicateNames = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.CategoryName.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key));

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;

                if (duplicateCodes.Contains(row.Code.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Code in Excel.";
                }
                if (duplicateNames.Contains(row.CategoryName.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Category Name in Excel.";
                }
            }

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK
            // Code exists in DB → IsExistingInDb = true
            // → stays in ValidRecords → REPLACE on confirm
            // =============================================
            var existingCodes = new HashSet<string>(
                _repository.GetAllCategoryCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower()));

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

        public int ImportCategories(List<CategoryDetailsImportRowDto> rows)
        {
            // Step 1 — Delete existing DB records being replaced
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();


            // Step 2 — Insert all valid rows (new + replaced)
            var categories = rows.Select(x => new CategoryDetails
            {
                Code = x.Code,
                CategoryName = x.CategoryName
            }).ToList();

            _repository.ReplaceCategories(codesToDelete,categories);
            return categories.Count;
        }
    }
}