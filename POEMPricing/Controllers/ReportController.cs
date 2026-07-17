using POEM.Model.Model.Report;
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
    [Authorize]
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


        #region ThumbnailProposal
        // Open ThumbnailProposal Report Page

        //[ActionName("ThumbnailProposal")]
        //public ActionResult SKUReport()
        //{
        //    LoadDropdowns();


        //    var data = _repo.GetSKUReport(
        //        null,
        //        null,
        //        null,
        //        null
        //    );


        //    return View("SKUReport",data);
        //}
        [ActionName("ThumbnailProposal")]
        public ActionResult SKUReport()
        {
            LoadDropdowns();

            var data = new List<SKUReportDto>();

            return View("SKUReport", data);
        }

        //[HttpGet]
        //public JsonResult GetSkus(
        //    string company,
        //    List<string> category,
        //    List<string> subCategory,
        //    List<string> collection)
        //{
        //    var data = _repo.GetSkus(
        //        company,
        //        category,
        //        subCategory,
        //        collection);

        //    return Json(
        //        data,
        //        JsonRequestBehavior.AllowGet);
        //}

        // Search
        //[HttpGet]
        //public ActionResult ThumbnailProposalReportSearch(
        //    string company,
        //    string category,
        //    string subCategory,
        //    string collection)
        //{

        //    LoadDropdowns();


        //    var data = _repo.GetSKUReport(
        //        company,
        //        category,
        //        subCategory,
        //        collection
        //    );


        //    return View("SKUReport", data);
        //}

        [HttpGet]
        public ActionResult ThumbnailProposalReportSearch(
            string company,
            List<string> category,
            List<string> subCategory,
            List<string> collection,
            List<string> skus,
            string price)
        {
            LoadDropdowns();
            ViewBag.SelectedCompany = company;
            ViewBag.SelectedCategories = category;
            ViewBag.SelectedSubCategories = subCategory;
            ViewBag.SelectedCollections = collection;
            ViewBag.SelectedSkus = skus;


            var data = _repo.GetSKUReport(
                company,
                category,
                subCategory,
                collection,
                skus
            );

            ViewBag.Price = price;

            return View("SKUReport", data);
        }

        public ActionResult ThumbnailProposalReportPdf(
        string company,
            List<string> category,
            List<string> subCategory,
            List<string> collection,
            List<string> skus,
            string price)
        {
            var data = _repo.GetSKUReport(
                company,
                category,
                subCategory,
                collection,
                skus
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
        #endregion


        #region CollectionProposal
        // Open CollectionProposal Report Page

        [ActionName("CollectionProposal")]
        public ActionResult CollectionProposal()
        {
            LoadDropdowns();


            var data = _repo.GetCollectionProposalReport(
                null,
                null
            );


            return View("CollectionProposal", data);
        }

        // Search
        [HttpGet]
        public ActionResult CollectionProposalReportSearch(
            string company,
            string collection)
        {

            LoadDropdowns();


            var data = _repo.GetCollectionProposalReport(
                company,
                collection
            );


            return View("CollectionProposal", data);
        }

        public ActionResult CollectionProposalReportPdf(
           string company,
           string collection,
           string price)
        {
            var data = _repo.GetCollectionProposalReport(
                company,
                collection
            );


            ViewBag.Price = price;


            return new ViewAsPdf("CollectionProposalPdf", data)
            {
                CustomSwitches =
                "--footer-center \"Page No- [page] of [topage]\" " +
                "--footer-font-size 10 " +
                "--footer-spacing 5"
            };
        }

        #endregion
    }
}