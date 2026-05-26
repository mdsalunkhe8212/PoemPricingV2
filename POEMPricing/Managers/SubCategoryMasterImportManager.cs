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
    public class SubCategoryMasterImportManager
    {
        private readonly IImportRepository _repository;

        public SubCategoryMasterImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<SubCategoryMasterRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<SubCategoryMasterRowDto>();
            var rows = new List<SubCategoryMasterRowDto>();

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
                        var subCategoryName = worksheet.Cell(row, 2).GetValue<string>().Trim();

                        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(subCategoryName))
                            continue;

                        rows.Add(new SubCategoryMasterRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            SubCategoryName = subCategoryName
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
                else if (string.IsNullOrWhiteSpace(row.SubCategoryName))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Sub Category Name is required.";
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

            // DUPLICATE INSIDE EXCEL - SUB CATEGORY NAME
            var duplicateNames = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.SubCategoryName.ToLower())
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
                if (duplicateNames.Contains(row.SubCategoryName.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Sub Category Name in Excel.";
                }
            }

            // DATABASE DUPLICATE CHECK
            var existingCodes = new HashSet<string>(
                _repository.GetAllSubCategoryMasterCodes()
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

        public int ImportSubCategories(List<SubCategoryMasterRowDto> rows)
        {
            var subCategories = rows.Select(x => new SubCategoryMasterDbDto
            {
                Code = x.Code,
                SubCategoryName = x.SubCategoryName
            }).ToList();

            _repository.BulkInsertSubCategoryMaster(subCategories);
            return subCategories.Count;
        }
    }
}