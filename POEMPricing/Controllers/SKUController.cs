using Newtonsoft.Json;
using POEM.Model.Model;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    [Authorize]

    public class SKUController : Controller
    {
        private readonly MasterDataRepository _masterDataRepository;
        private readonly SkuRepository _skuRepository;
        public SKUController()
        {
            var xmlPath = HostingEnvironment.MapPath("~/Config/MasterData.xml");
            _masterDataRepository = new MasterDataRepository(xmlPath);
            _skuRepository = new SkuRepository();
        }
        // GET: SKU List
        public ActionResult Index()
        {
            var model = new MasterDataModel
            {
                Companies = _masterDataRepository.GetDropdownFromDb("Company"),
                Collections = _masterDataRepository.GetDropdownFromDb("Collection"),
                Categories = _masterDataRepository.GetDropdownFromDb("Category"),
                SubCategories = _masterDataRepository.GetDropdownFromDb("SubCategory"),
               
            };
            return View(model);
        }
        // Add: SKU        
        public ActionResult Create()
        {
            var model = GetMasterData();
            return View(model);
        }
        private MasterDataModel GetMasterData()
        {
            return new MasterDataModel
            {
                Companies = _masterDataRepository.GetDropdownFromDb("Company"),
                OrderTypes = _masterDataRepository.GetDropdown("OrderTypes"),
                Labs = _masterDataRepository.GetDropdown("Lab"),
                GrowingTypes = _masterDataRepository.GetDropdown("GrowingType"),
                Collections = _masterDataRepository.GetDropdownFromDb("Collection"),
                Categories = _masterDataRepository.GetDropdownFromDb("Category"),
                SubCategories = _masterDataRepository.GetDropdownFromDb("SubCategory"),
                //SubCategories = new List<KeyValuePair<string, string>>
                //{
                //    new KeyValuePair<string, string>("", "")
                //},
                Vendors = _masterDataRepository.GetDropdownFromDb("ProductVendor"),
                Metals = _masterDataRepository.GetDropdown("MetalType"),
                Karats = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("24", "24K"),
                    new KeyValuePair<string, string>("18", "18K"),
                    new KeyValuePair<string, string>("14", "14K"),
                    new KeyValuePair<string, string>("10", "10K")
                },
                MetalColors = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "")
                },
                FindingTypes = _masterDataRepository.GetDropdownFromDb("FindingCCDll", "Type"),
                FindingSuppliers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "")
                },
                
                
                FindingAssemblys = _masterDataRepository.GetDropdownFromDb("FindingsAssembly"),
                StoneVendors = _masterDataRepository.GetDropdownFromDb("StoneVendor"),
                StoneTypes = _masterDataRepository.GetDropdownFromDb("StoneType"),
                SettingLocations = _masterDataRepository.GetDropdown("SettingLocation"),
                StoneShapes = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "")
                },
                StoneQualitys = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "")
                },
                SettingVendors = _masterDataRepository.GetDropdownFromDb("SettingVendor"),
                SettingTypes = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "")
                },
                LaborLocations = _masterDataRepository.GetDropdownFromDb("LaborLocation"),
                ProcessType = _masterDataRepository.GetDropdown("ProcessType"),
                OtherOption1 = _masterDataRepository.GetDropdownFromDb("OtherOption"),
                OtherOption2 = _masterDataRepository.GetDropdownFromDb("OtherOption"),
                OtherOption3 = _masterDataRepository.GetDropdownFromDb("OtherOption")


        };
        }
        public ActionResult Info(string skunumber)
        {
            if (string.IsNullOrWhiteSpace(skunumber))
            {
                return RedirectToAction("Index"); 
            }

            // ✅ Decode Base64 back to original SKU
            string decodedSku;
            try
            {
                byte[] data = Convert.FromBase64String(skunumber);
                decodedSku = Encoding.UTF8.GetString(data);
            }
            catch
            {
                return RedirectToAction("Index");
            }

            var masterData = GetMasterData(); 
            var skuModel = _skuRepository.GetSkuByNumber(decodedSku); 

            ViewBag.SkuModule = JsonConvert.SerializeObject(skuModel); // pass JSON to view
            return View("Create", masterData);
        }

        public ActionResult Edit(string skunumber)
        {
            // ✅ Decode Base64 back to original SKU
            string decodedSku;
            try
            {
                byte[] data = Convert.FromBase64String(skunumber);
                decodedSku = Encoding.UTF8.GetString(data);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            var masterData = GetMasterData(); // same as your Create() setup
            var skuModel = _skuRepository.GetSkuByNumber(decodedSku); // load JSON from DB

            ViewBag.SkuModule = JsonConvert.SerializeObject(skuModel); // pass JSON to view
            return View("Create", masterData);
        }

        public ActionResult Copy(string skunumber)
        {
            // ✅ Decode Base64 back to original SKU
            string decodedSku;
            try
            {
                byte[] data = Convert.FromBase64String(skunumber);
                decodedSku = Encoding.UTF8.GetString(data);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            var masterData = GetMasterData(); // same as your Create() setup
            var skuModel = _skuRepository.GetSkuByNumber(decodedSku); // load JSON from DB

            ViewBag.SkuModule = JsonConvert.SerializeObject(skuModel); // pass JSON to view
            return View("Create", masterData);
        }

    }
}