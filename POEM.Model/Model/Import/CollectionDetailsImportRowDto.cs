using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class CollectionDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }

        public string Collection { get; set; }
    }
}