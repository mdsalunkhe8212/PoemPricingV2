using Newtonsoft.Json;
using System;
using POEM.Model.Model;
using POEMPricing.Managers;
using POEM.Model.Model.Import;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    public class ImportController : Controller
    {
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateImport(ImportRequestDto model)
        {
            if (model.File == null || model.File.ContentLength == 0)
            {
                TempData["Error"] = "Please select excel file.";

                return RedirectToAction("Index");
            }

            if (model.MasterType == "CategoryDetails")
            {
                TempData["master"] = "Category Details";
                var manager = new CategoryDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["CategoryImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "SubCategoryMaster")
            {
                TempData["master"] = "Sub Category Master";
                var manager = new SubCategoryMasterImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["SubCategoryMasterImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "CollectionDetails")
            {
                TempData["master"] = "Collection Details";
                var manager = new CollectionDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["CollectionDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "CompanyMaster")
            {
                TempData["master"] = "Company Master";
                var manager = new CompanyMasterImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["CompanyMasterImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "FindingDetails")
            {
                TempData["master"] = "Finding Details";
                var manager = new FindingDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["FindingDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "StoneShapeDetails")
            {
                TempData["master"] = "Stone Shape Details";
                var manager = new StoneShapeDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["StoneShapeDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "SettingLaborDetails")
            {
                TempData["master"] = "Setting Labor Details";
                var manager = new SettingLaborDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["SettingLaborDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "VendorDetails")
            {
                TempData["master"] = "Vendor Details";
                var manager = new VendorDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["VendorDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "StoneQualityDetails")
            {
                TempData["master"] = "Stone Quality Details";
                var manager = new StoneQualityDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["StoneQualityDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            if (model.MasterType == "ProcessCostingDetails")
            {
                TempData["master"] = "Process Costing Details";
                var manager = new ProcessCostingDetailsImportManager();

                var result = manager.ValidateExcel(model.File);
                Session["ImportMasterType"] = model.MasterType;
                Session["ProcessCostingDetailsImportData"] =
                    JsonConvert.SerializeObject(result.ValidRecords);

                return View("Summary", result);
            }

            TempData["Error"] = "Invalid master type.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ConfirmImport()
        {
            var masterType = Convert.ToString(
                Session["ImportMasterType"]);

            if (string.IsNullOrEmpty(masterType))
            {
                TempData["Error"] = "Session expired.";

                return RedirectToAction("Upload");
            }

            // CATEGORY

            if (masterType == "CategoryDetails")
            {
                var sessionData = Session["CategoryImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<CategoryDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager = new CategoryDetailsImportManager();

                var insertedCount =
                    manager.ImportCategories(rows);

                Session.Remove("CategoryImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " category records imported successfully.";

                return RedirectToAction("Upload");
            }

            // SUB CATEGORY

            if (masterType == "SubCategoryMaster")
            {
                var sessionData =
                    Session["SubCategoryMasterImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<SubCategoryMasterRowDto>>
                    (sessionData.ToString());

                var manager =
                    new SubCategoryMasterImportManager();

                var insertedCount =
                    manager.ImportSubCategories(rows);

                Session.Remove("SubCategoryMasterImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " sub category records imported successfully.";

                return RedirectToAction("Upload");
            }

            //Collection details
            if (masterType == "CollectionDetails")
            {
                var sessionData =
                    Session["CollectionDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<CollectionDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new CollectionDetailsImportManager();

                var insertedCount =
                    manager.ImportCollections(rows);

                Session.Remove("CollectionDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " collection details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //Company Master
            if (masterType == "CompanyMaster")
            {
                var sessionData =
                    Session["CompanyMasterImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<CompanyMasterImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new CompanyMasterImportManager();

                var insertedCount =
                    manager.ImportCompanyMaster(rows);

                Session.Remove("CompanyMasterImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " company master records imported successfully.";

                return RedirectToAction("Upload");
            }

            //finding details
            if (masterType == "FindingDetails")
            {
                var sessionData =
                    Session["FindingDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<FindingDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new FindingDetailsImportManager();

                var insertedCount =
                    manager.ImportFindingDetails(rows);

                Session.Remove("FindingDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " finding details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //Stone shape details
            if (masterType == "StoneShapeDetails")
            {
                var sessionData =
                    Session["StoneShapeDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<StoneShapeDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new StoneShapeDetailsImportManager();

                var insertedCount =
                    manager.ImportStoneShapeDetails(rows);

                Session.Remove("StoneShapeDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " stone shape details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //SettingLaborDetails
            if (masterType == "SettingLaborDetails")
            {
                var sessionData =
                    Session["SettingLaborDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<SettingLaborDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new SettingLaborDetailsImportManager();

                var insertedCount =
                    manager.ImportSettingLaborDetails(rows);

                Session.Remove("SettingLaborDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " setting labour details details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //vendor details
            if (masterType == "VendorDetails")
            {
                var sessionData =
                    Session["VendorDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<VendorDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new VendorDetailsImportManager();

                var insertedCount =
                    manager.ImportVendorDetails(rows);

                Session.Remove("VendorDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " vendor details details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //StoneQualityDetails
            if (masterType == "StoneQualityDetails")
            {
                var sessionData =
                    Session["StoneQualityDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<StoneQualityDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new StoneQualityDetailsImportManager();

                var insertedCount =
                    manager.ImportStoneQualityDetails(rows);

                Session.Remove("StoneQualityDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " stone quality details details records imported successfully.";

                return RedirectToAction("Upload");
            }

            //ProcessCostingDetails
            if (masterType == "ProcessCostingDetails")
            {
                var sessionData =
                    Session["ProcessCostingDetailsImportData"];

                if (sessionData == null)
                {
                    TempData["Error"] = "Session expired.";

                    return RedirectToAction("Upload");
                }

                var rows = JsonConvert.DeserializeObject
                    <List<ProcessCostingDetailsImportRowDto>>
                    (sessionData.ToString());

                var manager =
                    new ProcessCostingDetailsImportManager();

                var insertedCount =
                    manager.ImportProcessCostingDetails(rows);

                Session.Remove("ProcessCostingDetailsImportData");

                Session.Remove("ImportMasterType");

                TempData["Success"] =
                    insertedCount + " process costing details details records imported successfully.";

                return RedirectToAction("Upload");
            }


            TempData["Error"] = "Invalid master type.";

            return RedirectToAction("Upload");
        }
    }
}