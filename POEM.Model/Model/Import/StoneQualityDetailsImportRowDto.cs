using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model.Import
{
    public class StoneQualityDetailsImportRowDto : BaseImportRowDto
    {
        public string Code { get; set; }
        public string CompanyCode { get; set; }
        public string StoneVendorCode { get; set; }
        public string StoneType { get; set; }
        public string StoneShapeCode { get; set; }
        public string StoneShape { get; set; }
        public string StoneQualityCode { get; set; }
        public string InternationalGrading { get; set; }
    }
}