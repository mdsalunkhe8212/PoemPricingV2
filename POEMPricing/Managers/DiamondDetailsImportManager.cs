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
    public class DiamondDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public DiamondDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<DiamondDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<DiamondDetailsImportRowDto>();
            var rows = new List<DiamondDetailsImportRowDto>();

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
                        "Code", "VendorCode", "StoneType", "GrowingType",
                        "StoneShapeCode", "StoneShape", "StoneQualityCode", "StoneQuality",
                        "SizeRange", "SizeFrom", "SizeTo", "SieveSize",
                        "LengthDiameter", "Width1", "Width2", "PerStoneWeight",
                        "StoneCertificate", "CostPerCt"
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
                            return result; // ← stop immediately, no further processing
                        }
                    }

                    // =============================================
                    // READ ROWS
                    // =============================================
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var code = worksheet.Cell(row, 1).GetValue<string>().Trim();
                        var vendorCode = worksheet.Cell(row, 2).GetValue<string>().Trim();
                        var stoneType = worksheet.Cell(row, 3).GetValue<string>().Trim();
                        var growingType = worksheet.Cell(row, 4).GetValue<string>().Trim();
                        var stoneShapeCode = worksheet.Cell(row, 5).GetValue<string>().Trim();
                        var stoneShape = worksheet.Cell(row, 6).GetValue<string>().Trim();
                        var stoneQualityCode = worksheet.Cell(row, 7).GetValue<string>().Trim();
                        var stoneQuality = worksheet.Cell(row, 8).GetValue<string>().Trim();
                        var sizeRange = worksheet.Cell(row, 9).GetValue<string>().Trim();
                        var sieveSize = worksheet.Cell(row, 12).GetValue<string>().Trim();
                        var stoneCertificate = worksheet.Cell(row, 17).GetValue<string>().Trim();

                        // Skip fully blank rows
                        if (string.IsNullOrWhiteSpace(code)
                            && string.IsNullOrWhiteSpace(vendorCode)
                            && string.IsNullOrWhiteSpace(stoneType)
                            && string.IsNullOrWhiteSpace(growingType)
                            && string.IsNullOrWhiteSpace(stoneShapeCode)
                            && string.IsNullOrWhiteSpace(stoneQuality)
                            && string.IsNullOrWhiteSpace(sizeRange))
                            continue;

                        rows.Add(new DiamondDetailsImportRowDto
                        {
                            RowNumber = row,

                            // Required in entity — pass "" (will be caught in Step 1 if blank)
                            Code = code ?? "",
                            VendorCode = vendorCode ?? "",
                            StoneType = stoneType ?? "",
                            GrowingType = growingType ?? "",
                            StoneShape = stoneShape ?? "",
                            StoneQualityCode = stoneQualityCode ?? "",
                            StoneQuality = stoneQuality ?? "",

                            // Optional in entity (no [Required]) — pass "" safely
                            StoneShapeCode = stoneShapeCode ?? "",
                            SizeRange = sizeRange ?? "",
                            SieveSize = sieveSize ?? "",
                            StoneCertificate = stoneCertificate ?? "",

                            // Decimals — TryGetValue → 0 if blank/garbage
                            SizeFrom = worksheet.Cell(row, 10).TryGetValue<decimal>(out var sf) ? sf : 0,
                            SizeTo = worksheet.Cell(row, 11).TryGetValue<decimal>(out var st) ? st : 0,
                            LengthDiameter = worksheet.Cell(row, 13).TryGetValue<decimal>(out var ld) ? ld : 0,
                            Width1 = worksheet.Cell(row, 14).TryGetValue<decimal>(out var w1) ? w1 : 0,
                            Width2 = worksheet.Cell(row, 15).TryGetValue<decimal>(out var w2) ? w2 : 0,
                            PerStoneWeight = worksheet.Cell(row, 16).TryGetValue<decimal>(out var psw) ? psw : 0,
                            CostPerCt = worksheet.Cell(row, 18).TryGetValue<decimal>(out var cpc) ? cpc : 0,
                        });
                    }
                }
            }

            result.TotalRows = rows.Count;

            // =============================================
            // STEP 1 — VALIDATION
            // Code is required + size 10
            // All other string fields: size check only (optional)
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
                else if (row.VendorCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Vendor Code cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.StoneType))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Type is required.";
                }
                else if (row.StoneType.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Type cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.GrowingType))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Growing Type is required.";
                }
                else if (row.GrowingType.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Growing Type cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.StoneShape))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Shape is required.";
                }
                else if (row.StoneShape.Length > 10)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Shape cannot exceed 10 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.StoneQualityCode))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Quality Code is required.";
                }
                else if (row.StoneQualityCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Quality Code cannot exceed 50 characters.";
                }
                else if (string.IsNullOrWhiteSpace(row.StoneQuality))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Quality is required.";
                }
                else if (row.StoneQuality.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Quality cannot exceed 50 characters.";
                }
                // ← optional fields below — size check only, no required check
                else if (row.StoneShapeCode.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Shape Code cannot exceed 50 characters.";
                }
                else if (row.SizeRange.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Size Range cannot exceed 50 characters.";
                }
                else if (row.SieveSize.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sieve Size cannot exceed 50 characters.";
                }
                else if (row.StoneCertificate.Length > 50)
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Stone Certificate cannot exceed 50 characters.";
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
                _repository.GetAllDiamondDetailsCodes()
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

        public int ImportDiamondDetails(List<DiamondDetailsImportRowDto> rows)
        {
            var codesToDelete = rows
                .Where(x => x.IsExistingInDb)
                .Select(x => x.Code)
                .ToList();

            var records = rows.Select(x => new DiamondDetail
            {
                Code = x.Code,
                VendorCode = x.VendorCode,
                StoneType = x.StoneType,
                GrowingType = x.GrowingType,
                StoneShapeCode = x.StoneShapeCode,
                StoneShape = x.StoneShape,
                StoneQualityCode = x.StoneQualityCode,
                StoneQuality = x.StoneQuality,
                SizeRange = x.SizeRange,
                SizeFrom = x.SizeFrom,
                SizeTo = x.SizeTo,
                SieveSize = x.SieveSize,
                LengthDiameter = x.LengthDiameter,
                Width1 = x.Width1,
                Width2 = x.Width2,
                PerStoneWeight = x.PerStoneWeight,
                StoneCertificate = x.StoneCertificate,
                CostPerCt = x.CostPerCt
            }).ToList();

            _repository.ReplaceDiamondDetails(codesToDelete, records);
            return records.Count;
        }

        public int GetCurrentCount()
        {
            return _repository.GetDiamondDetailsCount();
        }

        public List<DiamondDetail> GetCurrentRecords()
        {
            return _repository.GetAllDiamondDetailssRecords();
        }
    }
}