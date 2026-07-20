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
    public class DutyChartMasterImportManager
    {
        private readonly IImportRepository _repository;

        public DutyChartMasterImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<DutyChartMasterRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<DutyChartMasterRowDto>();
            var rows = new List<DutyChartMasterRowDto>();

            using (var stream = new MemoryStream())
            {
                file.InputStream.CopyTo(stream);

                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rowCount = worksheet.LastRowUsed().RowNumber();

                    // =============================================
                    // STEP 0 — VALIDATE COLUMN HEADERS
                    // =============================================
                    var expectedHeaders = new List<string>
                    {
                        "VendorLocation", "DutyPer", "TariffPer", "PenaltyPer"
                    };

                    for (int i = 0; i < expectedHeaders.Count; i++)
                    {
                        var actualHeader = worksheet.Cell(1, i + 1).GetValue<string>().Trim();

                        if (!actualHeader.Equals(expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                        {
                            result.IsValidTemplate = false;
                            result.TemplateError =
                                $"Invalid Excel template. " +
                                $"Expected column '{expectedHeaders[i]}' at position {i + 1} " +
                                $"but found '{(string.IsNullOrWhiteSpace(actualHeader) ? "empty" : actualHeader)}'.";
                            return result;
                        }
                    }

                    // =============================================
                    // READ ROWS
                    // =============================================
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var vendorLocation = worksheet.Cell(row, 1).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(vendorLocation)
                            && worksheet.Cell(row, 2).IsEmpty()
                            && worksheet.Cell(row, 3).IsEmpty()
                            && worksheet.Cell(row, 4).IsEmpty())
                            continue;

                        rows.Add(new DutyChartMasterRowDto
                        {
                            RowNumber = row,
                            VendorLocation = vendorLocation ?? "",

                            // value types — TryGetValue → 0 if empty/garbage
                            DutyPer = worksheet.Cell(row, 2).TryGetValue<decimal>(out var dp) ? dp : 0,
                            TariffPer = worksheet.Cell(row, 3).TryGetValue<decimal>(out var tp) ? tp : 0,
                            PenaltyPer = worksheet.Cell(row, 4).TryGetValue<decimal>(out var pp) ? pp : 0,
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // VendorLocation is the only required field
            // =============================================
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.VendorLocation))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Location is required.";
                }
                else if (row.VendorLocation.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor location cannot exceed 50 characters.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            // Same VendorLocation more than once → REJECTED
            // =============================================
            //var duplicates = new HashSet<string>(
            //    rows
            //    .Where(x => x.IsValid)
            //    .GroupBy(x => x.VendorLocation.ToLower())
            //    .Where(x => x.Count() > 1)
            //    .Select(x => x.Key)
            //);

            //foreach (var row in rows)
            //{
            //    if (!row.IsValid) continue;

            //    if (duplicates.Contains(row.VendorLocation.ToLower()))
            //    {
            //        row.IsDuplicate = true;
            //        row.ErrorMessage2 = "Duplicate Vendor Location in Excel.";
            //    }
            //}

            // =============================================
            // STEP 3 — NO PER-ROW DB CHECK
            // This master uses FULL REPLACE flow:
            // ALL existing DB records deleted on confirm
            // ALL valid Excel rows inserted fresh
            // No IsExistingInDb / IsNew distinction needed
            // =============================================
            var validRows = rows.Where(x => x.IsValid && !x.IsDuplicate).ToList();

            foreach (var row in validRows)
            {
                row.IsNew = true;
            }

            result.ValidRecords = validRows;
            result.InvalidRecords = rows.Where(x => !x.IsValid).ToList();
            result.DuplicateRecords = rows.Where(x => x.IsDuplicate).ToList();
            result.NewRecords = validRows;
            result.ExistingInDbRecords = new List<DutyChartMasterRowDto>(); // ← ADD

            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;
            result.NewRows = result.NewRecords.Count;
            result.ExistingInDbRows = _repository.GetDutyChartMasterCount(); // ← only once
            return result;
        }

        public int ImportDutyChartMaster(List<DutyChartMasterRowDto> rows)
        {
            // Full replace — delete ALL existing records, insert all valid rows
            var records = rows.Select(x => new DutyChartMasterDbDto
            {
                VendorLocation = x.VendorLocation,
                DutyPer = x.DutyPer,
                TariffPer = x.TariffPer,
                PenaltyPer = x.PenaltyPer
            }).ToList();

            // Single atomic transaction — delete all then insert all
            _repository.ReplaceDutyChartMaster(records);

            return records.Count;
        }

        public int GetCurrentCount()
        {
            return _repository.GetDutyChartMasterCount();
        }

        public List<DutyChartMasterDbDto> GetCurrentRecords()
        {
            return _repository.GetAllDutyChartMasterRecords();
        }
    }
}