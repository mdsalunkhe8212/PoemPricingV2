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
    public class DutyDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public DutyDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<DutyDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<DutyDetailsImportRowDto>();
            var rows = new List<DutyDetailsImportRowDto>();

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
                        "VendorLocation",
                        "Duty", "Tariff", "Penalty",
                        "DiamondLocation",
                        "DiamondDuty", "DiamondTariff", "DiamondPenalty",
                        "LaborLocation",
                        "LaborDuty", "LaborTariff", "LaborPenalty",
                        "FindingLocation",
                        "FindingDuty", "FindingTariff", "FindingPenalty"
                    };

                    for (int i = 0; i < expectedHeaders.Count; i++)
                    {
                        var actualHeader = worksheet.Cell(1, i + 1)
                            .GetValue<string>().Trim();

                        if (!actualHeader.Equals(expectedHeaders[i],
                            StringComparison.OrdinalIgnoreCase))
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
                        var dutyStr = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var tariffStr = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var penaltyStr = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var diamondLocation = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var diamondDutyStr = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var diamondTariffStr = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var diamondPenStr = worksheet.Cell(row, 8).GetValue<string>().Trim();
                        var laborLocation = worksheet.Cell(row, 9).GetValue<string>().Trim();
                        var laborDutyStr = worksheet.Cell(row, 10).GetValue<string>().Trim();
                        var laborTariffStr = worksheet.Cell(row, 11).GetValue<string>().Trim();
                        var laborPenStr = worksheet.Cell(row, 12).GetValue<string>().Trim();
                        var findingLocation = worksheet.Cell(row, 13).GetValue<string>().Trim();
                        var findingDutyStr = worksheet.Cell(row, 14).GetValue<string>().Trim();
                        var findingTariffStr = worksheet.Cell(row, 15).GetValue<string>().Trim();
                        var findingPenStr = worksheet.Cell(row, 16).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(vendorLocation)
                            && string.IsNullOrWhiteSpace(diamondLocation)
                            && string.IsNullOrWhiteSpace(laborLocation)
                            && string.IsNullOrWhiteSpace(findingLocation))
                            continue;

                        rows.Add(new DutyDetailsImportRowDto
                        {
                            RowNumber = row,

                            // string values
                            VendorLocation = vendorLocation ?? "",
                            DiamondLocation = diamondLocation ?? "",
                            LaborLocation = laborLocation ?? "",
                            FindingLocation = findingLocation ?? "",

                            // bool values — empty cell = false
                            Duty = dutyStr.Equals("true", StringComparison.OrdinalIgnoreCase) || dutyStr == "1",
                            Tariff = tariffStr.Equals("true", StringComparison.OrdinalIgnoreCase) || tariffStr == "1",
                            Penalty = penaltyStr.Equals("true", StringComparison.OrdinalIgnoreCase) || penaltyStr == "1",
                            DiamondDuty = diamondDutyStr.Equals("true", StringComparison.OrdinalIgnoreCase) || diamondDutyStr == "1",
                            DiamondTariff = diamondTariffStr.Equals("true", StringComparison.OrdinalIgnoreCase) || diamondTariffStr == "1",
                            DiamondPenalty = diamondPenStr.Equals("true", StringComparison.OrdinalIgnoreCase) || diamondPenStr == "1",
                            LaborDuty = laborDutyStr.Equals("true", StringComparison.OrdinalIgnoreCase) || laborDutyStr == "1",
                            LaborTariff = laborTariffStr.Equals("true", StringComparison.OrdinalIgnoreCase) || laborTariffStr == "1",
                            LaborPenalty = laborPenStr.Equals("true", StringComparison.OrdinalIgnoreCase) || laborPenStr == "1",
                            FindingDuty = findingDutyStr.Equals("true", StringComparison.OrdinalIgnoreCase) || findingDutyStr == "1",
                            FindingTariff = findingTariffStr.Equals("true", StringComparison.OrdinalIgnoreCase) || findingTariffStr == "1",
                            FindingPenalty = findingPenStr.Equals("true", StringComparison.OrdinalIgnoreCase) || findingPenStr == "1",
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
                else if (string.IsNullOrWhiteSpace(row.DiamondLocation))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Diamond Location is required.";
                }
                else if (string.IsNullOrWhiteSpace(row.LaborLocation))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "labour location is required.";
                }
                else if (string.IsNullOrWhiteSpace(row.FindingLocation))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "finding  location is required.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            //// =============================================
            //// STEP 2 — DUPLICATE CHECK WITHIN EXCEL
            //// Same VendorLocation more than once → REJECTED
            //// =============================================
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
            // Full replace flow:
            // ALL existing DB records deleted on confirm
            // ALL valid Excel rows inserted fresh
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
            result.ExistingInDbRecords = new List<DutyDetailsImportRowDto>(); // ← ADD

            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;
            result.NewRows = result.NewRecords.Count;
            result.ExistingInDbRows = _repository.GetDutyDetailsCount(); // ← only once
            return result;
        }

        public int ImportDutyDetails(List<DutyDetailsImportRowDto> rows)
        {
            // Full replace — delete ALL then insert ALL valid rows
            var records = rows.Select(x => new DutyDetailsDbDto
            {
                VendorLocation = x.VendorLocation,
                Duty = x.Duty,
                Tariff = x.Tariff,
                Penalty = x.Penalty,
                DiamondLocation = x.DiamondLocation,
                DiamondDuty = x.DiamondDuty,
                DiamondTariff = x.DiamondTariff,
                DiamondPenalty = x.DiamondPenalty,
                LaborLocation = x.LaborLocation,
                LaborDuty = x.LaborDuty,
                LaborTariff = x.LaborTariff,
                LaborPenalty = x.LaborPenalty,
                FindingLocation = x.FindingLocation,
                FindingDuty = x.FindingDuty,
                FindingTariff = x.FindingTariff,
                FindingPenalty = x.FindingPenalty
            }).ToList();

            _repository.ReplaceDutyDetails(records);
            return records.Count;
        }

        public int GetCurrentCount()
        {
            return _repository.GetDutyDetailsCount();
        }

        public List<DutyDetailsDbDto> GetCurrentRecords()
        {
            return _repository.GetAllDutyDetailsRecords();
        }
    }
}