using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class ImportRequestDto
    {
        public string MasterType { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}