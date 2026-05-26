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
    public class CollectionDetailsImportManager
    {
        private readonly IImportRepository _repository;

        public CollectionDetailsImportManager()
        {
            _repository = new ImportRepository();
        }

        public ImportSummaryDto<CollectionDetailsImportRowDto> ValidateExcel(HttpPostedFileBase file)
        {
            var result = new ImportSummaryDto<CollectionDetailsImportRowDto>();
            var rows = new List<CollectionDetailsImportRowDto>();

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
                        var collection = worksheet.Cell(row, 2).GetValue<string>().Trim();

                        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(collection))
                            continue;

                        rows.Add(new CollectionDetailsImportRowDto
                        {
                            RowNumber = row,
                            Code = code,
                            Collection = collection
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
                else if (string.IsNullOrWhiteSpace(row.Collection))
                {
                    row.IsValid = false;
                    row.ErrorMessage1 = "Collection is required.";
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

            // DUPLICATE INSIDE EXCEL - COLLECTION
            var duplicateCollections = new HashSet<string>(
                rows.Where(x => x.IsValid)
                .GroupBy(x => x.Collection.ToLower())
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
                if (duplicateCollections.Contains(row.Collection.ToLower()))
                {
                    row.IsDuplicate = true;
                    row.ErrorMessage2 = "Duplicate Collection in Excel.";
                }
            }

            // DATABASE DUPLICATE CHECK
            var existingCodes = new HashSet<string>(
                _repository.GetAllCollectionDetailsCodes()
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

        public int ImportCollections(List<CollectionDetailsImportRowDto> rows)
        {
            var collections = rows.Select(x => new CollectionDtl
            {
                Code = x.Code,
                Collection = x.Collection
            }).ToList();

            _repository.BulkInsertCollectionDetailsMaster(collections);
            return collections.Count;
        }
    }
}