using POEM.Services.Interface;
using POEM.Services.Repository;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace POEMPricing.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepository _repo;
        private readonly MasterDataRepository _masterDataRepository;


        public ReportController()
        {
            var xmlPath = HostingEnvironment.MapPath("~/Config/MasterData.xml");
            _masterDataRepository = new MasterDataRepository(xmlPath);
            _repo = new ReportRepository();
        }


        // Load dropdown common method
        private void LoadDropdowns()
        {
            ViewBag.Companies =
                _masterDataRepository.GetDropdownFromDb("Company");

            ViewBag.Collections =
                _masterDataRepository.GetDropdownFromDb("Collection");

            ViewBag.Categories =
                _masterDataRepository.GetDropdownFromDb("Category");

            ViewBag.SubCategories =
                _masterDataRepository.GetDropdownFromDb("SubCategory");
        }



        // Open Report Page
        public ActionResult SKUReport()
        {
            LoadDropdowns();


            var data = _repo.GetSKUReport(
                null,
                null,
                null,
                null
            );


            return View(data);
        }



        // Search
        [HttpGet]
        public ActionResult SKUReportSearch(
            string company,
            string category,
            string subCategory,
            string collection)
        {

            LoadDropdowns();


            var data = _repo.GetSKUReport(
                company,
                category,
                subCategory,
                collection
            );


            return View("SKUReport", data);
        }

        public ActionResult DownloadSKUReportPdf(
        string company,
        string category,
        string subCategory,
        string collection,
        string price)
        {
            var data = _repo.GetSKUReport(
                company,
                category,
                subCategory,
                collection
            );


            ViewBag.Price = price;


            return new ViewAsPdf("SKUReportPdf", data)
            {
                CustomSwitches =
                "--footer-center \"Page No- [page] of [topage]\" " +
                "--footer-font-size 10 " +
                "--footer-spacing 5"
            };
        }
    }
}