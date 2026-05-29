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

                        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(companyName))
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

            // VALIDATION
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
                else
                {
                    row.IsValid = true;
                }
            }

            // DUPLICATE INSIDE EXCEL - CODE
            var duplicateCodes = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.Code.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key));

            // DUPLICATE INSIDE EXCEL - COMPANY NAME
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

            // DATABASE DUPLICATE CHECK
            var existingCodes = new HashSet<string>(
                _repository.GetAllCompanyMasterCodes()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToLower()));

            foreach (var row in rows)
            {
                if (!row.IsValid) continue;

                if (existingCodes.Contains(row.Code.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Code already exists in database.";
                }
            }

            result.ValidRecords = rows.Where(x => x.IsValid && !x.IsDuplicate).ToList();
            result.InvalidRecords = rows.Where(x => !x.IsValid).ToList();
            result.DuplicateRecords = rows.Where(x => x.IsDuplicate).ToList();
            result.ValidRows = result.ValidRecords.Count;
            result.InvalidRows = result.InvalidRecords.Count;
            result.DuplicateRows = result.DuplicateRecords.Count;

            return result;
        }

        public int ImportCompanyMaster(List<CompanyMasterImportRowDto> rows)
        {
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