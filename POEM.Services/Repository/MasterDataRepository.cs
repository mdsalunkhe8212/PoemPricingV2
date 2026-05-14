using POEM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using POEM.Model.Model;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;


namespace POEM.Services.Repository
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly string _xmlPath;
        private readonly ApplicationDbContext _context;        
        public MasterDataRepository(string xmlPath) 
        {
            _xmlPath = xmlPath;
            _context = new ApplicationDbContext();
        }       

        public List<KeyValuePair<string, string>> GetDropdown(string category)
        {
            var doc = XDocument.Load(_xmlPath);
            var items = doc.Descendants(category)
                .Elements("Item")
                .Select(x => new KeyValuePair<string, string>(
                    x.Attribute("Value")?.Value ?? "",
                    x.Value))
                .ToList();
            var rtnItems = items.OrderBy(x => x.Value);
            return rtnItems.ToList() ;
        }

        public List<KeyValuePair<string, string>> GetDropdownFromDb(string category, string param = "",string param1="", string param2 = "", string param3 = "", string param4 = "")
        {
            List<KeyValuePair<string, string>> items = null;
            switch (category)
            {

                case "OtherOption":
                    if (param == "" && param1 == "")
                    {
                        items = new List<KeyValuePair<string, string>>();
                        items.Add(new KeyValuePair<string, string>("", ""));
                    }else
                    {
                        items = _context.ProcessCostingDetails
                            .Where(c => c.VendorCode == param && c.IsOptional==true && (c.Category == param1 ||c.Category.ToLower() == "all"))
                            .Select(c => new
                            {
                                Key = c.Type.ToString(),
                                Value = c.Type
                            })
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    }
                    break;
                case "Collection":
                    items = _context.CollectionDtls
                        .Select(c => new
                        {
                            Key = c.Code.ToString(),
                            Value = c.Collection
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "Company":
                    items = _context.CompanyMaster
                        .Select(c => new
                        {
                            Key = c.Code.ToString(),
                            Value = c.CompanyName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "Category":
                    
                        // Return top-level categories (no parent)
                        items = _context.CategoryDetails
                            
                            .Select(c => new
                            {
                                Key = c.Code.ToString(),
                                Value = c.CategoryName
                            })
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    break;
                case "SubCategory":
                   
                        // Return subcategories for specific parent ID
                        
                        items = _context.SubCategoryMasters
                           
                            .Select(c => new
                            {
                                Key = c.Code.ToString(),
                                Value = c.SubCategoryName
                            })
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    
                    break;
                case "ProductVendor":
                    items = _context.VendorDetails             
                        .Where(c=> c.ProductVendor == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;

                case "FindingsSupplier":
                    items = _context.VendorDetails
                         .Where(c => c.FindingsSupplier == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "FindingsAssembly":
                    items = _context.VendorDetails
                         .Where(c => c.FindingsAssembly == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "StoneVendor":
                    items = _context.VendorDetails
                         .Where(c => c.StoneVendor == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "SettingVendor":
                    items = _context.VendorDetails
                         .Where(c => c.SettingVendor == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "LabourLocation":
                    items = _context.VendorDetails
                         .Where(c => c.LabourLocation == true)
                        .Select(c => new
                        {
                            Key = c.VendorCode.Trim()+"|"+c.VendorLocation.Trim(),
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;

                case "MetelColor":
                    items = _context.MetelColorDtls
                        .Where(c => c.MetalType == param)
                        .Select(c => new
                        {
                            Key = c.MetelColor,
                            Value = c.MetelColor
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "FindingSupplier":
                    items = _context.findingDetails
                        .Select(f => new
                        {
                            Key = f.FindingSupplier,
                            Value = f.FindingSupplier
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "FindingType"://FindingDescription
                    items = _context.findingDetails
                        .Where(c => c.FindingSupplier == param)
                        .Select(f => new
                        {
                            Key = f.FindingType,
                            Value = f.FindingType
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "FindingCCDll"://FindingDescription
                    if (param == "Type") { 
                    items = _context.findingDetails
                        .Select(f => new
                        {
                            Key = f.FindingType,
                            Value = f.FindingType
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    }else if (param == "Metal")
                    {
                        items = _context.findingDetails
                            .Where(c => c.FindingType == param1)
                            .Select(f => new
                            {
                                Key = f.FindingMetalType,
                                Value = f.FindingMetalType
                            })
                            .Distinct()
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    }
                    else if (param == "Karat")
                    {
                        items = _context.findingDetails
                            .Where(c => c.FindingType == param1 && c.FindingMetalType==param2)
                            .Select(f => new
                            {
                                Key = f.FindingMetalKt,
                                Value = f.FindingMetalKt
                            })
                            .Distinct()
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    }
                    else if (param == "Color")
                    {
                        items = _context.findingDetails
                            .Where(c => c.FindingType == param1 && c.FindingMetalType == param2 && (c.FindingMetalKt==param3 || c.FindingMetalKt == c.FindingMetalKt))
                            .Select(f => new
                            {
                                Key = f.FindingMetalColor,
                                Value = f.FindingMetalColor
                            })
                            .Distinct()
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    }
                    else if (param == "Supplier")
                    {
                        items = _context.findingDetails
                            .Where(c => c.FindingType == param1 && c.FindingMetalType == param2 && (c.FindingMetalKt == param3 || c.FindingMetalKt == "") && c.FindingMetalColor==param4)
                            .Select(f => new
                            {
                                Key = f.FindingSupplier,
                                Value = f.FindingVendorName
                            })
                            .Distinct()
                            .ToList()  // Execute SQL first
                            .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                            .ToList();
                    }
                    break;
                case "FindingAssembly":
                    items = _context.VendorDetails
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorCode
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "FindingDescriptionSKU":
                    var parts = param.Split('|');          // do this in memory
                    var supplier = parts[0];
                    var type = parts[1];
                    var search = parts[2];
                    items = _context.findingDetails
                        .Where(c => c.FindingSupplier == supplier 
                        && c.FindingType == type
                        && (c.FindingDescription.Contains(search) || c.FindingNumber.Contains(search)))
                        .Select(f => new
                        {
                            Key = f.FindingDescription,
                            Value = f.FindingNumber
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                //case "StoneVendor":
                //    items = _context.VendorDetails
                //        .Select(c => new
                //        {
                //            Key = c.VendorCode,
                //            Value = c.VendorCode
                //        })
                //        .ToList()  // Execute SQL first
                //        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                //        .ToList();
                //    break;
                case "StoneType":                    
                    items = _context.StoneShapeDetails
                        .Select(f => new
                        {
                            Key = f.StoneType,
                            Value = f.StoneType
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "StoneShape":
                    items = _context.StoneShapeDetails
                        .Where(s => s.StoneType == param)
                        .Select(f => new
                        {
                            Key = f.Code.ToString(),
                            Value = f.StoneShape
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "MetalLoss":
                    var metalLossParts = param.Split('-');          // do this in memory
                    var vendor = metalLossParts[0];
                    var metaltype = metalLossParts[1];
                    
                    items = _context.MetalLossDetails
                        .Where(c => c.VendorCode == vendor
                        && c.MetalType == metaltype)                        
                        .Select(f => new
                        {
                            Key = f.MetalType,
                            Value = f.LossPer.ToString()
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                case "StoneQuality":
                    var stoneQualityParts = param.Split('|');          // do this in memory
                    var stonetype = stoneQualityParts[0];
                    var growingtype = stoneQualityParts[1];
                    var stoneshape = stoneQualityParts[2];
                    items = _context.DiamondDetails
                        .Where(c => c.StoneType == stonetype
                        && c.GrowingType == growingtype
                        && c.StoneShape == stoneshape)
                        .Select(f => new
                        {
                            Key = f.StoneQuality,
                            Value = f.StoneQuality
                        })
                        .Distinct()
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                //case "SettingVendor":
                //    items = _context.SettingLaborDetails
                //        .Select(f => new
                //        {
                //            Key = f.SettingVendor,
                //            Value = f.SettingVendor
                //        })
                //        .Distinct()
                //        .ToList()  // Execute SQL first
                //        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                //        .ToList();
                //    break;
                case "SettingType":
                    var settingVendor = param;
                    items = _context.SettingLaborDetails
                         .AsNoTracking()
                         .Where(s => s.SettingVendor == settingVendor)
                         .GroupBy(f => f.SettingType)
                         .Select(g => new
                         {
                             Key = g.FirstOrDefault().Code,
                             Value = g.Key   
                         })
                         .ToList()
                         .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                         .ToList();
                    break;
                case "LaborLocation":
                    items = _context.VendorDetails
                        .Select(c => new
                        {
                            Key = c.VendorCode,
                            Value = c.VendorName
                        })
                        .ToList()  // Execute SQL first
                        .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                        .ToList();
                    break;
                default:
                    items = new List<KeyValuePair<string, string>>();
                    break;
            }
            var rtnItems = items.OrderBy(x => x.Value);
            return rtnItems.ToList();
        }

        public FindingDetail GetFindingDetails(string findingnumber)
        {
            FindingDetail findingDetail = _context.findingDetails
                .FirstOrDefault(f => f.FindingNumber == findingnumber);
            return findingDetail;
        }



        public VendorDetails GetVendorDetails(string VendorCode)
        {
            VendorDetails vendorDetails = _context.VendorDetails
                .FirstOrDefault(v => v.VendorCode == VendorCode);
            return vendorDetails;
        }
        public ProcessCostingDetails GetProcessCost(string vendor, string processType, string category)
        {
            var costps = null as decimal?;
            ProcessCostingDetails processCostingDetails = _context.ProcessCostingDetails
                .FirstOrDefault(s => s.VendorCode.Trim().ToLower() == vendor.Trim().ToLower() &&
    s.Type.Trim().ToLower() == processType.Trim().ToLower() &&
    (s.Category.Trim().ToLower() == category.Trim().ToLower() || s.Category.Trim().ToLower() == "all"));

            return processCostingDetails;


        }

        public MarginDetailsDbDto GetMarginDetails(string vendor, string category, string subcategory, string metal)
        {
            MarginDetailsDbDto marginDetails = _context.MarginDetails
                .FirstOrDefault(m => m.Vendor == vendor && m.CategoryCode==category && m.SubCategoryCode == subcategory  && m.Metal== metal);

            if (marginDetails==null) {
                marginDetails = _context.MarginDetails
                .FirstOrDefault(m => m.Vendor == vendor && m.CategoryCode == category && m.SubCategoryCode == subcategory && m.Metal.ToLower().Trim() == "all");
            }
            return marginDetails;
        }

        public Task<decimal?> GetPerStoneWeight(string stoneType, string growingType, string stoneShape, string lengthDiameter)
        {
            // normalize inputs if needed
            var ld = (lengthDiameter ?? string.Empty).Trim();
            if(!decimal.TryParse(ld, out decimal ldValue))
            {
                return Task.FromResult<decimal?>(null);
            }
            var ldDecimal = ldValue;
            var result = _context.DiamondDetails
                .AsNoTracking()
                .Where(dd =>
                    dd.StoneType == stoneType &&
                    dd.GrowingType == growingType &&
                    dd.StoneShape == stoneShape &&
                    dd.LengthDiameter == ldDecimal)
                .Select(dd => (decimal?)dd.PerStoneWeight) // cast to nullable if column can be null
                .FirstOrDefaultAsync();
            return result;

        }

        public Task<decimal?> GetStoneCostPerCarat(string vendor,string stoneType, string growingType, string stoneShape, string lengthDiameter, string stoneQuality)
        {
            // normalize inputs
            var ld = (lengthDiameter ?? string.Empty).Trim();

            // if length/diameter is not a valid decimal, return null immediately
            if (!decimal.TryParse(ld, out decimal ldValue))
            {
                return Task.FromResult<decimal?>(null);
            }

            var ldDecimal = ldValue;

            // query using numeric comparison against the decimal column
            var result = _context.DiamondDetails
                .AsNoTracking()
                .Where(dd =>
                    dd.StoneType == stoneType &&
                    dd.GrowingType == growingType &&
                    dd.StoneShape == stoneShape &&
                    dd.LengthDiameter == ldDecimal &&
                    dd.StoneQuality == stoneQuality)
                .Select(dd => (decimal?)dd.CostPerCt)
                .FirstOrDefaultAsync();

            return result;
        }
        public Task<decimal?> GetSettingCostPerStone(string vendor, string settingType, decimal perStoneWt, string shape, string category, string subCategory)
        {
            var costps= null as decimal?;
            var rows = _context.SettingLaborDetails
                .AsNoTracking()
                .Where(s => s.SettingVendor == vendor &&
                            s.SettingType == settingType &&
                            (s.Shape == shape || s.Shape == "All Shape") &&
                            //(s.Category == category || s.Category == "") &&
                            //(s.SubCategory == subCategory || s.SubCategory == "") &&
                            (s.DiamondPSWtFrom <= perStoneWt && s.DiamondPSWtTo >= perStoneWt))
                .FirstOrDefault();
                //.ToList();   // <-- IMPORTANT: Split only works after this

            //foreach (var row in rows)
            //{
            //    if (string.IsNullOrWhiteSpace(row.DiamondPSWtRange))
            //        continue;

            //    var parts = row.DiamondPSWtRange.Split('-');
            //    if (parts.Length != 2)
            //        continue;

            //    if (decimal.TryParse(parts[0], out decimal min) &&
            //        decimal.TryParse(parts[1], out decimal max))
            //    {
            //        if (perStoneWt >= min && perStoneWt <= max)
            //        {
            //            costps = row.GoldCostPS;   // or whichever cost column you want
            //            return Task.FromResult(costps);
            //        }
            //    }
            //}

                if (rows != null) {
                costps = rows.GoldCostPS;
                return Task.FromResult(costps);
            }

            return null;


        }

       
    }
}