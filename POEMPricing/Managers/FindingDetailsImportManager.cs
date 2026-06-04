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
    public class FindingDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public FindingDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<FindingDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<FindingDetailsImportRowDto>();
            var rows = new List<FindingDetailsImportRowDto>();

            using (var stream = new MemoryStream())
            {
                file.InputStream.CopyTo(stream);

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rowCount = worksheet.LastRowUsed().RowNumber();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Read all columns first
                        var findingSupplier = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var findingVendorName = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var findingVendorCode = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var company = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var findingNumber = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var findingMetalType = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var findingMetalKt = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var findingMetalColor = worksheet.Cell(row, 8).GetValue<string>().Trim();
                        var findingType = worksheet.Cell(row, 9).GetValue<string>().Trim();
                        var findingDescription = worksheet.Cell(row, 10).GetValue<string>().Trim();
                        var findingShortDescription = worksheet.Cell(row, 11).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(findingSupplier)
                            && string.IsNullOrWhiteSpace(findingVendorName)
                            && string.IsNullOrWhiteSpace(findingVendorCode)
                            && string.IsNullOrWhiteSpace(company)
                            && string.IsNullOrWhiteSpace(findingNumber)
                            && string.IsNullOrWhiteSpace(findingMetalType)
                            && string.IsNullOrWhiteSpace(findingDescription))
                            continue;

                        rows.Add(new FindingDetailsImportRowDto
                        {
                            RowNumber = row,
                            FindingSupplier = findingSupplier,
                            FindingVendorName = findingVendorName,
                            FindingVendorCode = findingVendorCode,
                            Company = company,
                            FindingNumber = findingNumber,
                            FindingMetalType = findingMetalType,
                            FindingMetalKt = findingMetalKt,
                            FindingMetalColor = findingMetalColor,
                            FindingType = findingType,
                            FindingDescription = findingDescription,
                            FindingShortDescription = findingShortDescription,
                            PerPcFindingWeightGms = worksheet.Cell(row, 12).GetValue<decimal>(),
                            Increment = worksheet.Cell(row, 13).GetValue<decimal>(),
                            Decrement = worksheet.Cell(row, 14).GetValue<decimal>(),
                            MetalLock = worksheet.Cell(row, 15).GetValue<int>(),
                            FindingCost = worksheet.Cell(row, 16).GetValue<decimal>()
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // FindingNumber is the unique key
            // =============================================
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.FindingNumber))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Finding Number is required.";
                }
                else if (row.FindingNumber.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding number cannot exceed 10 characters.";
                }
                else if (row.FindingSupplier.Length > 100)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding supplier cannot exceed 100 characters.";
                }
                else if (row.FindingVendorCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding vendor code cannot exceed 10 characters.";
                }
                else if (row.FindingVendorName.Length > 100)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding vendor name cannot exceed 100 characters.";
                }
                else if (row.Company.Length > 100)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "company cannot exceed 100 characters.";
                }
                else if (row.FindingMetalType.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding metal type cannot exceed 50 characters.";
                }
                else if (row.FindingMetalKt.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding metal kt cannot exceed 10 characters.";
                }
                else if (row.FindingMetalColor.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding metal color cannot exceed 50 characters.";
                }
                else if (row.FindingType.Length > 100)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding type cannot exceed 100 characters.";
                }
                else if (row.FindingDescription.Length > 255)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding description cannot exceed 255 characters.";
                }
                else if (row.FindingShortDescription.Length > 255)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding short disc cannot exceed 255 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // Same FindingNumber appears more than once
            // in the uploaded sheet → REJECTED
            // =============================================
            var duplicateFindingNumbers = new HashSet<string>(
                rows
                .Where(x => x.IsValid)
                .GroupBy(x => x.FindingNumber.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (duplicateFindingNumbers.Contains(row.FindingNumber.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Finding Number in Excel.";
                }
            }

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK
            // FindingNumber exists in DB
            // → IsExistingInDb = true → stays in ValidRecords
            // → On confirm: old deleted, new inserted (REPLACE)
            // =============================================
            var existingFindingNumbers = new HashSet<string>(
                _repository.GetAllFindingDetailsFindingNumbers()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (row.IsDuplicate) continue;

                if (existingFindingNumbers.Contains(row.FindingNumber.ToLower()))
                {
                    row.IsExistingInDb = true;  // ← BUG FIX: was missing!
                    row.ErrorMessage3 = "Finding Number already exists in DB — will be replaced.";
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
            //result.NewRows = rows.Count(x => x.IsValid && !x.IsDuplicate && !x.IsExistingInDb
            result.NewRows = result.NewRecords.Count;

            return result;
        }

        public int ImportFindingDetails(List<FindingDetailsImportRowDto> rows)
        {
            // Step 1 — Delete existing DB records for FindingNumbers being replaced
            var numbersToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.FindingNumber)
                .ToList();

            if (numbersToDelete.Any())
                _repository.DeleteFindingDetailsByFindingNumbers(numbersToDelete);

            // Step 2 — Insert all valid rows (new + replaced)
            var records = rows.Select(x => new FindingDetail
            {
                FindingSupplier = x.FindingSupplier,
                FindingVendorName = x.FindingVendorName,
                FindingVendorCode = x.FindingVendorCode,
                Company = x.Company,
                FindingNumber = x.FindingNumber,
                FindingMetalType = x.FindingMetalType,
                FindingMetalKt = x.FindingMetalKt,
                FindingMetalColor = x.FindingMetalColor,
                FindingType = x.FindingType,
                FindingDescription = x.FindingDescription,
                FindingShortDescription = x.FindingShortDescription,
                PerPcFindingWeightGms = x.PerPcFindingWeightGms,
                Increment = x.Increment,
                Decrement = x.Decrement,
                MetalLock = x.MetalLock,
                FindingCost = x.FindingCost
            }).ToList();

            _repository.BulkInsertFindingDetails(records);
            return records.Count;
        }
    }
}