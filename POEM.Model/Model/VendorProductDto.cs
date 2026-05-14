using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class VendorProductDto
	{
        public long skuId { get; set; }
        public string company { get; set; }
        public string skuNumber { get; set; }
        public string vendor { get; set; }
        public string vendorNumber { get; set; }
        public string orderType { get; set; }
        public string categoryCode { get; set; }
        public string category { get; set; }
        public string subCategoryCode { get; set; }
        public string subCategory { get; set; }
        public string collectionCode { get; set; }
        public string collection { get; set; }
        public string sizeLength { get; set; }
        public string mmWidth { get; set; }
        public string mmHeight { get; set; }
        public string semiMinWt { get; set; }
        public string centerMinWt { get; set; }
        public string SemiAdjWt { get; set; }
        public string CenterAdjWt { get; set; }

    }
}