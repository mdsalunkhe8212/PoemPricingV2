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
    public class VendorDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public VendorDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<VendorDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<VendorDetailsImportRowDto>();
            var rows = new List<VendorDetailsImportRowDto>();

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
                        "VendorLocation", "VendorName", "VendorCode",
                        "DiamondHandlingLab", "DiaHndLabLow", "DiaHndLabHigh",
                        "DiamondHandlingMined", "DiaHndMinedLow", "DiaHndMinedHigh",
                        "FindingHndGold", "FindingHndPlatinum", "FindingHndSilver",
                        "ModelMkgGold", "ModelMkgPlatinum", "ModelMkgSilver",
                        "CAMGold", "CAMPlatinum", "CAMSilver",
                        "ProductVendor", "FindingsSupplier", "FindingsAssembly",
                        "StoneVendor", "SettingVendor", "LabourLocation"
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
                        var vendorLocation = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var vendorName = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var vendorCode = worksheet.Cell(row, 3).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(vendorLocation)
                            && string.IsNullOrWhiteSpace(vendorName)
                            && string.IsNullOrWhiteSpace(vendorCode))
                            continue;

                        var diaHndLabStr = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var diaHndLabLowStr = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var diaHndLabHiStr = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var diaHndMinStr = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var diaHndMinLowStr = worksheet.Cell(row, 8).GetValue<string>().Trim();
                        var diaHndMinHiStr = worksheet.Cell(row, 9).GetValue<string>().Trim();
                        var fndHndGoldStr = worksheet.Cell(row, 10).GetValue<string>().Trim();
                        var fndHndPlatStr = worksheet.Cell(row, 11).GetValue<string>().Trim();
                        var fndHndSilvStr = worksheet.Cell(row, 12).GetValue<string>().Trim();
                        var mdlMkgGoldStr = worksheet.Cell(row, 13).GetValue<string>().Trim();
                        var mdlMkgPlatStr = worksheet.Cell(row, 14).GetValue<string>().Trim();
                        var mdlMkgSilvStr = worksheet.Cell(row, 15).GetValue<string>().Trim();
                        var camGoldStr = worksheet.Cell(row, 16).GetValue<string>().Trim();
                        var camPlatStr = worksheet.Cell(row, 17).GetValue<string>().Trim();
                        var camSilvStr = worksheet.Cell(row, 18).GetValue<string>().Trim();
                        var productVendor = worksheet.Cell(row, 19).GetValue<string>().Trim();
                        var findingsSupp = worksheet.Cell(row, 20).GetValue<string>().Trim();
                        var findingsAssem = worksheet.Cell(row, 21).GetValue<string>().Trim();
                        var stoneVendor = worksheet.Cell(row, 22).GetValue<string>().Trim();
                        var settingVendor = worksheet.Cell(row, 23).GetValue<string>().Trim();
                        var labourLocation = worksheet.Cell(row, 24).GetValue<string>().Trim();

                        rows.Add(new VendorDetailsImportRowDto
                        {
                            RowNumber = row,
                            VendorLocation = vendorLocation,
                            VendorName = vendorName,
                            VendorCode = vendorCode,
                            DiamondHandlingLab = decimal.TryParse(diaHndLabStr, out var v1) ? v1 : (decimal?)null,
                            DiaHndLabLow = decimal.TryParse(diaHndLabLowStr, out var v2) ? v2 : (decimal?)null,
                            DiaHndLabHigh = decimal.TryParse(diaHndLabHiStr, out var v3) ? v3 : (decimal?)null,
                            DiamondHandlingMined = decimal.TryParse(diaHndMinStr, out var v4) ? v4 : (decimal?)null,
                            DiaHndMinedLow = decimal.TryParse(diaHndMinLowStr, out var v5) ? v5 : (decimal?)null,
                            DiaHndMinedHigh = decimal.TryParse(diaHndMinHiStr, out var v6) ? v6 : (decimal?)null,
                            FindingHndGold = decimal.TryParse(fndHndGoldStr, out var v7) ? v7 : (decimal?)null,
                            FindingHndPlatinum = decimal.TryParse(fndHndPlatStr, out var v8) ? v8 : (decimal?)null,
                            FindingHndSilver = decimal.TryParse(fndHndSilvStr, out var v9) ? v9 : (decimal?)null,
                            ModelMkgGold = decimal.TryParse(mdlMkgGoldStr, out var v10) ? v10 : (decimal?)null,
                            ModelMkgPlatinum = decimal.TryParse(mdlMkgPlatStr, out var v11) ? v11 : (decimal?)null,
                            ModelMkgSilver = decimal.TryParse(mdlMkgSilvStr, out var v12) ? v12 : (decimal?)null,
                            CAMGold = decimal.TryParse(camGoldStr, out var v13) ? v13 : (decimal?)null,
                            CAMPlatinum = decimal.TryParse(camPlatStr, out var v14) ? v14 : (decimal?)null,
                            CAMSilver = decimal.TryParse(camSilvStr, out var v15) ? v15 : (decimal?)null,
                            ProductVendor = productVendor.Equals("true", StringComparison.OrdinalIgnoreCase) || productVendor == "1",
                            FindingsSupplier = findingsSupp.Equals("true", StringComparison.OrdinalIgnoreCase) || findingsSupp == "1",
                            FindingsAssembly = findingsAssem.Equals("true", StringComparison.OrdinalIgnoreCase) || findingsAssem == "1",
                            StoneVendor = stoneVendor.Equals("true", StringComparison.OrdinalIgnoreCase) || stoneVendor == "1",
                            SettingVendor = settingVendor.Equals("true", StringComparison.OrdinalIgnoreCase) || settingVendor == "1",
                            LabourLocation = labourLocation.Equals("true", StringComparison.OrdinalIgnoreCase) || labourLocation == "1",
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // Check if VendorCode is present
            // If blank → IsValid = false → goes to InvalidRecords
            // If present → IsValid = true → continues to next checks
            // =============================================
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.VendorCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Code is required.";
                }
                else if (row.VendorCode.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Code cannot exceed 10 characters.";
                }
                else if (row.VendorLocation.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "vendor location cannot exceed 50 characters.";
                }
                else if (row.VendorName.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "vendor name cannot exceed 50 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // If same VendorCode appears more than once
            // in the uploaded Excel sheet itself
            // → IsDuplicate = true → goes to DuplicateRecords
            // → REJECTED — not inserted at all
            // =============================================
            var codesInExcel = new HashSet<string>(
                rows
                .Where(x => x.IsValid) // only check valid rows
                .GroupBy(x => x.VendorCode.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue; // skip invalid rows

                if (codesInExcel.Contains(row.VendorCode.ToLower()))
                {
                    // Mark as duplicate — will be excluded from import
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Vendor Code in Excel.";
                }
            }

            // =============================================
            // STEP 3 — DATABASE EXISTING CHECK (NEW BEHAVIOUR)
            // If VendorCode already exists in DB
            // → OLD BEHAVIOUR: IsDuplicate = true → rejected
            // → NEW BEHAVIOUR: IsExistingInDb = true → still in ValidRecords
            //   → On confirm: old record DELETED from DB, new one INSERTED
            //   → This is a REPLACE operation, not a reject
            // =============================================
            var codesInDb = new HashSet<string>(
                _repository.GetAllVendorDetailsCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;  // skip invalid rows
                if (row.IsDuplicate) continue; // skip in-sheet duplicates

                if (codesInDb.Contains(row.VendorCode.ToLower()))
                {
                    // NEW: mark as existing in DB — NOT rejected
                    // This row will still be in ValidRecords
                    // On import: old DB record deleted, this new one inserted
                    row.IsExistingInDb = true;
                    row.ErrorMessage3 = "Vendor Code exists in DB — will be replaced.";
                }
                else
                {
                    // Not in DB  new record
                    row.IsNew = true; // ← NEW
                }
            }

            // =============================================
            // STEP 4 — SEPARATE INTO RESULT BUCKETS
            //
            // ValidRecords     = IsValid=true AND IsDuplicate=false
            //                    (includes both NEW and EXISTING-IN-DB rows)
            //
            // InvalidRecords   = IsValid=false (blank VendorCode)
            //
            // DuplicateRecords = IsDuplicate=true (repeated in Excel sheet)
            //
            // ExistingInDbRecords = IsExistingInDb=true (will be replaced)
            //
            // NewRows          = valid rows that are brand new (not in DB)
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

        public int ImportVendorDetails(List<VendorDetailsImportRowDto> rows)
        {
            // =============================================
            // STEP 1 — DELETE existing DB records
            // Find all rows marked as IsExistingInDb = true
            // These are rows where VendorCode already exists in DB
            // Delete them first before inserting new data
            // =============================================
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.VendorCode)
                .ToList();

            if (codesToDelete.Any())
            {
                // Delete old records from DB for these VendorCodes
                _repository.DeleteVendorDetailsByCodes(codesToDelete);
            }

            // =============================================
            // STEP 2 — INSERT all valid rows
            // This includes:
            //   - Brand new VendorCodes (never in DB)
            //   - Replaced VendorCodes (old deleted above, new inserted now)
            // =============================================
            var records = rows.Select(x => new VendorDetails
            {
                VendorLocation = x.VendorLocation,
                VendorName = x.VendorName,
                VendorCode = x.VendorCode,
                DiamondHandlingLab = x.DiamondHandlingLab,
                DiaHndLabLow = x.DiaHndLabLow,
                DiaHndLabHigh = x.DiaHndLabHigh,
                DiamondHandlingMined = x.DiamondHandlingMined,
                DiaHndMinedLow = x.DiaHndMinedLow,
                DiaHndMinedHigh = x.DiaHndMinedHigh,
                FindingHndGold = x.FindingHndGold,
                FindingHndPlatinum = x.FindingHndPlatinum,
                FindingHndSilver = x.FindingHndSilver,
                ModelMkgGold = x.ModelMkgGold,
                ModelMkgPlatinum = x.ModelMkgPlatinum,
                ModelMkgSilver = x.ModelMkgSilver,
                CAMGold = x.CAMGold,
                CAMPlatinum = x.CAMPlatinum,
                CAMSilver = x.CAMSilver,
                ProductVendor = x.ProductVendor,
                FindingsSupplier = x.FindingsSupplier,
                FindingsAssembly = x.FindingsAssembly,
                StoneVendor = x.StoneVendor,
                SettingVendor = x.SettingVendor,
                LabourLocation = x.LabourLocation
            }).ToList();

            _repository.BulkInsertVendorDetails(records);
            return records.Count;
        }
    }
}