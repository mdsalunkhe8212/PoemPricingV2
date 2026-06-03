using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class BaseImportRowDto
    {
        public int RowNumber { get; set; }

        public bool IsValid { get; set; } = true;

        public bool IsDuplicate { get; set; }
        public bool IsExistingInDb { get; set; }

        public bool IsNew { get; set; }
        public string ErrorMessage1 { get; set; }

        public string ErrorMessage2 { get; set; }

        public string ErrorMessage3 { get; set; }
    }
}