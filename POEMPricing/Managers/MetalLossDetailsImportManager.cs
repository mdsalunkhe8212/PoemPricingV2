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
    public class MetalLossDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public MetalLossDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<MetalLossDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<MetalLossDetailsImportRowDto>();
            var rows = new List<MetalLossDetailsImportRowDto>();

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
                        "VendorCode", "VendorName", "MetalType", "LossPer"
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
                        var vendorCode = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var vendorName = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var metalType = worksheet.Cell(row, 3).GetValue<string>().Trim();

                        // Skip fully blank rows across all mapped columns
                        if (string.IsNullOrWhiteSpace(vendorCode)
                            && string.IsNullOrWhiteSpace(vendorName)
                            && string.IsNullOrWhiteSpace(metalType)
                            && worksheet.Cell(row, 4).IsEmpty())
                            continue;

                        rows.Add(new MetalLossDetailsImportRowDto
                        {
                            RowNumber = row,
                            VendorCode = vendorCode ?? "",
                            VendorName = vendorName ?? "",
                            MetalType = metalType ?? "",

                            // Value type — TryGetValue → 0 if empty/garbage
                            LossPer = worksheet.Cell(row, 4).TryGetValue<decimal>(out var lp) ? lp : 0,
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
                if (string.IsNullOrWhiteSpace(row.VendorCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Code is required.";
                }
                else if (row.VendorCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Code cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.VendorName))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Name is required.";
                }
                else if (row.VendorName.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Name cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.MetalType))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Metal Type is required.";
                }
                else if (row.VendorCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Metal Type cannot exceed 50 characters.";
                }
                else if (row.LossPer == 0)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "LossPer is required.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // =============================================
            // STEP 2 — DUPLICATE CHECK WITHIN EXCEL not req here
            

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
            result.ExistingInDbRecords = new List<MetalLossDetailsImportRowDto>(); // ← ADD

            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;
            result.NewRows = result.NewRecords.Count;
            result.ExistingInDbRows = _repository.GetMetalLossDetailsCount(); // ← only once
            return result;
        }

        public int ImportMetalLossDetails(List<MetalLossDetailsImportRowDto> rows)
        {
            // Full replace — delete ALL existing records, insert all valid rows
            var records = rows.Select(x => new MetalLossDetails
            {
                VendorCode = x.VendorCode,
                VendorName = x.VendorName,
                MetalType = x.MetalType,
                LossPer = x.LossPer
            }).ToList();

            // Single atomic transaction — delete all then insert all
            _repository.ReplaceMetalLossDetails(records);

            return records.Count;
        }

        public int GetCurrentCount()
        {
            return _repository.GetMetalLossDetailsCount();
        }

        public List<MetalLossDetails> GetCurrentRecords()
        {
            return _repository.GetAllMetalLossDetailsRecords();
        }
    }
}