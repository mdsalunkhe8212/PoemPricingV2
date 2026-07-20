using ClosedXML.Excel;
using Newtonsoft.Json;
using POEM.Model.Model;
using POEM.Model.Model.Import;
using POEMPricing.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImportController : Controller
    {
        private readonly ExportManager _exportManager = new ExportManager(); // for export to excel

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

            try
            {
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

                if (model.MasterType == "MarginDetails")
                {
                    TempData["master"] = "Margin Details";
                    var manager = new MarginDetailsImportManager();

                    var result = manager.ValidateExcel(model.File);
                    Session["ImportMasterType"] = model.MasterType;
                    Session["MarginDetailsImportData"] =
                        JsonConvert.SerializeObject(result.ValidRecords);

                    return View("Summary", result);
                }

                if (model.MasterType == "DiamondDetails")
                {
                    TempData["master"] = "Diamond Details";
                    var manager = new DiamondDetailsImportManager();

                    var result = manager.ValidateExcel(model.File);
                    Session["ImportMasterType"] = model.MasterType;
                    Session["DiamondDetailsImportData"] =
                        JsonConvert.SerializeObject(result.ValidRecords);

                    return View("Summary", result);
                }

                if (model.MasterType == "DutyChartMaster")
                {
                    TempData["master"] = "Duty Chart Master";
                    var manager = new DutyChartMasterImportManager();
                    var result = manager.ValidateExcel(model.File);
                    Session["ImportMasterType"] = model.MasterType;
                    Session["DutyChartMasterImportData"] =
                        JsonConvert.SerializeObject(result.ValidRecords);
                    return View("Summary", result);
                }

                if (model.MasterType == "DutyDetails")
                {
                    TempData["master"] = "Duty Details";
                    var manager = new DutyDetailsImportManager();
                    var result = manager.ValidateExcel(model.File);
                    Session["ImportMasterType"] = model.MasterType;
                    Session["DutyDetailsImportData"] =
                        JsonConvert.SerializeObject(result.ValidRecords);
                    return View("Summary", result);
                }

                TempData["Error"] = "Invalid master type.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // TODO: log ex somewhere (file/DB/Application Insights)

                TempData["Error"] = "Unable to read the Excel file. " +
                    "Please make sure the file is a valid .xlsx and try again.";

                return RedirectToAction("Upload");
            }
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

            try
            {
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

                //MarginDetails
                if (masterType == "MarginDetails")
                {
                    var sessionData =
                        Session["MarginDetailsImportData"];

                    if (sessionData == null)
                    {
                        TempData["Error"] = "Session expired.";

                        return RedirectToAction("Upload");
                    }

                    var rows = JsonConvert.DeserializeObject
                        <List<MarginDetailsImportRowDto>>
                        (sessionData.ToString());

                    var manager =
                        new MarginDetailsImportManager();

                    var insertedCount =
                        manager.ImportMarginDetails(rows);

                    Session.Remove("MarginDetailsImportData");

                    Session.Remove("ImportMasterType");

                    TempData["Success"] =
                        insertedCount + " margin details records imported successfully.";

                    return RedirectToAction("Upload");
                }


                //diamonddetails
                if (masterType == "DiamondDetails")
                {
                    var sessionData =
                        Session["DiamondDetailsImportData"];

                    if (sessionData == null)
                    {
                        TempData["Error"] = "Session expired.";

                        return RedirectToAction("Upload");
                    }

                    var rows = JsonConvert.DeserializeObject
                        <List<DiamondDetailsImportRowDto>>
                        (sessionData.ToString());

                    var manager =
                        new DiamondDetailsImportManager();

                    var insertedCount =
                        manager.ImportDiamondDetails(rows);

                    Session.Remove("DiamondDetailsImportData");

                    Session.Remove("ImportMasterType");

                    TempData["Success"] =
                        insertedCount + " Diamond Details records imported successfully.";

                    return RedirectToAction("Upload");
                }


                //dutychart 
                if (masterType == "DutyChartMaster")
                {
                    var sessionData = Session["DutyChartMasterImportData"];
                    if (sessionData == null)
                    {
                        TempData["Error"] = "Session expired.";
                        return RedirectToAction("Upload");
                    }
                    var rows = JsonConvert.DeserializeObject
                        <List<DutyChartMasterRowDto>>(sessionData.ToString());
                    var manager = new DutyChartMasterImportManager();
                    var insertedCount = manager.ImportDutyChartMaster(rows);
                    Session.Remove("DutyChartMasterImportData");
                    Session.Remove("ImportMasterType");
                    TempData["Success"] =
                        insertedCount + " duty chart master records imported successfully.";
                    return RedirectToAction("Upload");
                }

                //dutydetails
                if (masterType == "DutyDetails")
                {
                    var sessionData = Session["DutyDetailsImportData"];
                    if (sessionData == null)
                    {
                        TempData["Error"] = "Session expired.";
                        return RedirectToAction("Upload");
                    }
                    var rows = JsonConvert.DeserializeObject
                        <List<DutyDetailsImportRowDto>>(sessionData.ToString());
                    var manager = new DutyDetailsImportManager();
                    var insertedCount = manager.ImportDutyDetails(rows);
                    Session.Remove("DutyDetailsImportData");
                    Session.Remove("ImportMasterType");
                    TempData["Success"] =
                        insertedCount + " duty details records imported successfully.";
                    return RedirectToAction("Upload");
                }


                // ← FIX: fallback for unmatched master type — was missing before,
                // caused "not all code paths return a value" compiler error
                TempData["Error"] = "Invalid master type.";

                return RedirectToAction("Upload");
            }
            catch (Exception ex)
            {
                // TODO: log ex somewhere (file/DB/Application Insights)

                TempData["Error"] = "Something went wrong while importing records. " +
                    "No data was changed. Please try again or contact support.";

                return RedirectToAction("Upload");
            }
        }


        #region excelexport
        [HttpGet]
        public ActionResult ExportToExcel(string masterType)
        {
            if (string.IsNullOrEmpty(masterType))
            {
                TempData["Error"] = "Please select a master.";
                return RedirectToAction("Upload");
            }

            try
            {
                var fileBytes = _exportManager.ExportToExcel(masterType);

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{masterType}.xlsx");
            }
            catch (Exception ex)
            {
                // TODO: log ex somewhere (file/DB/Application Insights)

                TempData["Error"] = "Unable to export records. Please try again or contact support.";
                return RedirectToAction("Upload");
            }
        }

        [HttpGet]
        public JsonResult GetExportCount(string masterType)
        {
            try
            {
                var count = _exportManager.GetCount(masterType);
                return Json(new { count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // TODO: log ex somewhere (file/DB/Application Insights)

                return Json(new { count = 0, error = "Unable to fetch count." },
                    JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}