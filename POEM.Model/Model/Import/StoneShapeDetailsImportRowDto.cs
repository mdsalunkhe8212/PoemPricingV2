using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class StoneShapeDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string StoneType { get; set; }

        public string StoneShape { get; set; }
        public string CategoryFancyRound { get; set; }
    }
}