using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class ImportSummaryDto<T>
        where T : BaseImportRowDto
    {
        public int TotalRows { get; set; }

        public int ValidRows { get; set; }

        public int DuplicateRows { get; set; }

        public int InvalidRows { get; set; }

        public int ExistingInDbRows { get; set; }  // ← NEW
        public int NewRows { get; set; }

        public List<T> ValidRecords { get; set; }

        public List<T> DuplicateRecords { get; set; }

        public List<T> InvalidRecords { get; set; }
        public List<T> ExistingInDbRecords { get; set; }

        public List<T> NewRecords { get; set; }
        public ImportSummaryDto()
        {
            ValidRecords = new List<T>();

            DuplicateRecords = new List<T>();
            
            InvalidRecords = new List<T>();
            ExistingInDbRecords = new List<T>();

            NewRecords = new List<T>();
        }
    }
}