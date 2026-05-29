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
                        var findingNumber = worksheet.Cell(row, 5)
                            .GetValue<string>()
                            .Trim();



                        //if (string.IsNullOrWhiteSpace(findingNumber))
                        //{
                        //    continue;
                        //}

                        var dto = new FindingDetailsImportRowDto
                        {
                            RowNumber = row,
                            FindingSupplier = worksheet.Cell(row, 1).GetValue<string>().Trim(),
                            FindingVendorName = worksheet.Cell(row, 2).GetValue<string>().Trim(),
                            FindingVendorCode = worksheet.Cell(row, 3).GetValue<string>().Trim(),
                            Company = worksheet.Cell(row, 4).GetValue<string>().Trim(),
                            FindingNumber = findingNumber,
                            FindingMetalType = worksheet.Cell(row, 6).GetValue<string>().Trim(),
                            FindingMetalKt = worksheet.Cell(row, 7).GetValue<string>().Trim(),
                            FindingMetalColor = worksheet.Cell(row, 8).GetValue<string>().Trim(),
                            FindingType = worksheet.Cell(row, 9).GetValue<string>().Trim(),
                            FindingDescription = worksheet.Cell(row, 10).GetValue<string>().Trim(),
                            PerPcFindingWeightGms = worksheet.Cell(row, 11).GetValue<decimal>(),
                            Increment = worksheet.Cell(row, 12).GetValue<decimal>(),
                            Decrement = worksheet.Cell(row, 13).GetValue<decimal>(),
                            MetalLock = worksheet.Cell(row, 14).GetValue<int>(),
                            FindingCost = worksheet.Cell(row, 15).GetValue<decimal>()
                        };

                        rows.Add(dto);
                    }
                }
            }

            result.TotalRows = rows.Count;

            // VALIDATION

            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.FindingNumber))
                {
                    row.IsValid = false;

                    row.ErrorMessage1 = "Finding Number is required.";
                }
                else
                {
                    row.IsValid = true;
                }
            }

            // DUPLICATE INSIDE EXCEL - CODE

            var duplicateFindingNumbers = new HashSet<string>(
                rows
                .Where(x => x.IsValid)  // ← ADD THIS
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

            // DATABASE DUPLICATE CHECK


            var existingFindingNumbers = new HashSet<string>(
                _repository.GetAllFindingDetailsFindingNumbers()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower())
            );


            foreach (var row in rows)
            {
                if (!row.IsValid) continue;
                if (existingFindingNumbers.Contains(row.FindingNumber.ToLower()))
                {
                    row.IsDuplicate = true;

                    row.ErrorMessage2 = "Finding Numbers already exists in database.";
                }


            }

            result.ValidRecords = rows
                .Where(x => x.IsValid && !x.IsDuplicate)
                .ToList();

            result.InvalidRecords = rows
                .Where(x => !x.IsValid)
                .ToList();

            result.DuplicateRecords = rows
                .Where(x => x.IsDuplicate)
                .ToList();

            result.ValidRows = result.ValidRecords.Count;

            result.InvalidRows = result.InvalidRecords.Count;

            result.DuplicateRows = result.DuplicateRecords.Count;

            return result;
        }

        public int ImportFindingDetails(List<FindingDetailsImportRowDto> rows)
        {
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