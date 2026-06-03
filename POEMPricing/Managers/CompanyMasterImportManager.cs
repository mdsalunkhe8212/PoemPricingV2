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
    public class CompanyMasterImportManager
    {
        private readonly IImportRepository _repository;

        public CompanyMasterImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<CompanyMasterImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<CompanyMasterImportRowDto>();
            var rows = new List<CompanyMasterImportRowDto>();

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
                        var companyName = worksheet.Cell(row, 2).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(companyName))
                            continue;

                        rows.Add(new CompanyMasterImportRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            CompanyName = companyName
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
                else if (string.IsNullOrWhiteSpace(row.CompanyName))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Company Name is required.";
                }
                else if (row.Code.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code cannot exceed 10 characters.";
                }
                else if (row.CompanyName.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Comapany Name cannot exceed 50 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // =============================================
            var duplicateCodes = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.Code.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key));

            var duplicateNames = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.CompanyName.ToLower())
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
                if (duplicateNames.Contains(row.CompanyName.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Company Name in Excel.";
                }
            }

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK
            // Code exists in DB → IsExistingInDb = true → REPLACE
            // =============================================
            var existingCodes = new HashSet<string>(
                _repository.GetAllCompanyMasterCodes()
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

        public int ImportCompanyMaster(List<CompanyMasterImportRowDto> rows)
        {
            // Step 1 — Delete existing DB records being replaced
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();

            if (codesToDelete.Any())
                _repository.DeleteCompanyMasterByCodes(codesToDelete);

            // Step 2 — Insert all valid rows (new + replaced)
            var records = rows.Select(x => new CompanyMasterDbDto
            {
                Code = x.Code,
                CompanyName = x.CompanyName
            }).ToList();

            _repository.BulkInsertCompanyMaster(records);
            return records.Count;
        }
    }
}