using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POEM.Model.Model
{
	public class MasterDataModel
	{
        public string SelectedCompany { get; set; }
        public string SelectedOrderType { get; set; }
        public string SelectedLab { get; set; }
        public string SelectedGrowingType { get; set; }
        public string SelectedCollection { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedSubCategory { get; set; }
        public string SelectedVendor { get; set; }
        public string SelectedMetal { get; set; }
        public string SelectedKarat { get; set; }
        public string SelectedMetalColors { get; set; }
        public string SelectedFindingSupplier { get; set; }
        public string SelectedFindingType { get; set; }
        public string SelectedFindingAssembly { get; set; }
        public string SelectedStoneVendor { get; set; }
        public string SelectedStoneType { get; set; }
        public string SelectedStoneShape { get; set; }
        public string SelectedSettingLocation { get; set; }
        public string SelectedStoneQuality { get; set; }
        public string SelectedSettingVendor { get; set; }
        public string SelectedSettingType { get; set; }
        public string SelectedLaborLocation { get; set; }
        public string SelectedProcessType { get; set; }
        public List<KeyValuePair<string,string>> Companies { get; set; }
        public List<KeyValuePair<string, string>> OrderTypes { get; set; }
        public List<KeyValuePair<string, string>> Labs { get; set; }        
        public List<KeyValuePair<string, string>> GrowingTypes { get; set; }
        public List<KeyValuePair<string, string>> Collections { get; set; }
        public List<KeyValuePair<string, string>> Categories { get; set; }
        public List<KeyValuePair<string, string>> SubCategories { get; set; }
        public List<KeyValuePair<string, string>> Vendors { get; set; }
        public List<KeyValuePair<string, string>> Metals { get; set; }
        public List<KeyValuePair<string, string>> Karats { get; set; }
        public List<KeyValuePair<string, string>> MetalColors { get; set; }
        public List<KeyValuePair<string, string>> FindingSuppliers { get; set; }
        public List<KeyValuePair<string, string>> FindingTypes { get; set; }
        public List<KeyValuePair<string, string>> FindingAssemblys { get; set; }
        public List<KeyValuePair<string, string>> StoneVendors { get; set; }
        public List<KeyValuePair<string, string>> StoneTypes { get; set; }
        public List<KeyValuePair<string, string>> StoneShapes { get; set; }
        public List<KeyValuePair<string, string>> SettingLocations { get; set; }
        public List<KeyValuePair<string, string>> StoneQualitys { get; set; }
        public List<KeyValuePair<string, string>> SettingVendors { get; set; }
        public List<KeyValuePair<string, string>> SettingTypes { get; set; }
        public List<KeyValuePair<string, string>> LaborLocations { get; set; }
        public List<KeyValuePair<string, string>> ProcessType { get; set; }
        public List<KeyValuePair<string, string>> OtherOption1 { get; set; }
        public List<KeyValuePair<string, string>> OtherOption2 { get; set; }
        public List<KeyValuePair<string, string>> OtherOption3 { get; set; }



    }
}