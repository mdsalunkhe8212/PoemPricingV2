using Microsoft.Ajax.Utilities;
using POEM.Model.Model;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace POEMPricing.API
{
    [RoutePrefix("api/sku")]
    public class SKUController : ApiController
    {
        private readonly HttpClient httpclient;
        public string webRoot;
        private readonly MasterDataRepository _masterDataRepository;
        private readonly SkuRepository _skuRepository;
        public SKUController()
        {
            webRoot = System.Configuration.ConfigurationManager.AppSettings["WebRoot"];
            httpclient = new HttpClient();
            var xmlPath = "";
            _masterDataRepository = new MasterDataRepository(xmlPath);
            _skuRepository = new SkuRepository();
        }
        //GET: api/sku/subcategory
        [HttpGet]
        [Route("subcategory")]
        public async Task<IHttpActionResult> subcategory(string ParentId)
        {
            try
            {
                var subCategories = _masterDataRepository.GetDropdownFromDb("SubCategory", ParentId.ToString());
                await Task.Delay(0);
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //GET: api/sku/subcategory
        [HttpGet]
        [Route("otherOptional")]
        public async Task<IHttpActionResult> otherOptional([FromUri] string vendorcode, [FromUri] string category)
        {
            try
            {
                var otherOptional = _masterDataRepository.GetDropdownFromDb("OtherOption", vendorcode, category);
                await Task.Delay(0);
                return Ok(otherOptional);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //GET: api/sku/metalcolor
        [HttpGet]
        [Route("metalcolor")]
        public async Task<IHttpActionResult> metalcolor([FromUri] string metelType)
        {
            try
            {
                var metalColor = _masterDataRepository.GetDropdownFromDb("MetelColor", metelType);
                await Task.Delay(0);
                return Ok(metalColor);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/findingtype
        [HttpGet]
        [Route("findingtype/{supplier}")]
        public async Task<IHttpActionResult> findingtype([FromUri] string supplier)
        {
            try
            {
                var subCategories = _masterDataRepository.GetDropdownFromDb("FindingType", supplier);
                await Task.Delay(0);
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/findingdropdown
        [HttpGet]
        [Route("findingdropdown/{field}/{filter?}/{filter1?}/{filter2?}/{filter3?}")]
        public async Task<IHttpActionResult> findingDropdown([FromUri] string field, [FromUri] string filter="", [FromUri] string filter1="", [FromUri] string filter2 = "", [FromUri] string filter3 = "")
        {
            try
            {
                var subCategories = _masterDataRepository.GetDropdownFromDb("FindingCCDll", field, filter, filter1, filter2, filter3);
                await Task.Delay(0);
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/findingdescription
        [HttpGet]
        [Route("findingdescription/{supplier}/{type}/{search}")]
        public async Task<IHttpActionResult> findingdescription([FromUri] string supplier, [FromUri] string type, [FromUri] string search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(search) || search.Length < 5)
                    return Ok();
                var subCategories = _masterDataRepository.GetDropdownFromDb("FindingDescriptionSKU", supplier + "|" + type + "|" + search);
                await Task.Delay(0);
                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/stoneshape
        [HttpGet]
        [Route("stoneshape/{stonetype}")]
        public async Task<IHttpActionResult> stoneshape([FromUri] string stonetype)
        {
            try
            {
                var stoneType = _masterDataRepository.GetDropdownFromDb("StoneShape", stonetype);
                await Task.Delay(0);
                return Ok(stoneType);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/findingdetails
        [HttpGet]
        [Route("findingdetails/{findingnumber}")]
        public async Task<IHttpActionResult> findingdetails([FromUri] string findingnumber)
        {
            try
            {
                FindingDetail findingDetail = _masterDataRepository.GetFindingDetails(findingnumber);
                await Task.Delay(0);
                return Ok(findingDetail);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("vendordetails/{VendorCode}")]
        public async Task<IHttpActionResult> VendorDetails([FromUri] string VendorCode)
        {
            try
            {
                VendorDetails vendorDetails = _masterDataRepository.GetVendorDetails(VendorCode);
                await Task.Delay(0);
                return Ok(vendorDetails);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("getprocesscost/{vendor}/{processType}/{category}")]
        public async Task<IHttpActionResult> GetProcessCost([FromUri] string vendor, [FromUri] string processType, [FromUri] string category)
        {
            try
            {
                ProcessCostingDetails processCostingDetails = _masterDataRepository.GetProcessCost(vendor, processType, category);
                await Task.Delay(0);
                return Ok(processCostingDetails);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //GET: api/sku/metalloss
        [HttpGet]
        [Route("metalloss/{vendor}/{metal}")]
        public async Task<IHttpActionResult> metalloss([FromUri] string vendor, string metal)
        {
            try
            {
                var metalLoss = _masterDataRepository.GetDropdownFromDb("MetalLoss", vendor + '-' + metal);
                await Task.Delay(0);
                return Ok(metalLoss);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/margindetails
        [HttpGet]
        [Route("margindetails/{vendor}/{category}/{subcategory}/{metal}")]
        public async Task<IHttpActionResult> MarginDetails([FromUri] string vendor, [FromUri] string category, [FromUri] string subcategory, [FromUri] string metal)
        {
            try
            {
                MarginDetailsDbDto marginDetails = _masterDataRepository.GetMarginDetails(vendor,category,subcategory,metal);
                await Task.Delay(0);
                return Ok(marginDetails);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        //GET: api/sku/stonequality
        [HttpGet]
        [Route("stonequality/{StoneType}/{GrowingType}/{StoneShape}")]
        public async Task<IHttpActionResult> stonequality([FromUri] string stonetype, string growingtype, string stoneshape)
        {
            try
            {
                if (growingtype == "Lab - HPHT  CVD") { growingtype = "Lab - HPHT / CVD"; }
                ;
                var stoneQuality = _masterDataRepository.GetDropdownFromDb("StoneQuality", stonetype + '|' + growingtype + '|' + stoneshape);
                await Task.Delay(0);
                return Ok(stoneQuality);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        // GET api/sku/perstoneweight?stoneType=...&growingType=...&stoneShape=...&lengthDiameter=...
        [HttpGet]
        [Route("perstoneweight")]
        public async Task<IHttpActionResult> GetPerStoneWeight([FromUri] string stoneType, [FromUri] string growingType, [FromUri] string stoneShape, [FromUri] string lengthDiameter)
        {
            if (string.IsNullOrWhiteSpace(stoneType) || string.IsNullOrWhiteSpace(stoneShape) || string.IsNullOrWhiteSpace(lengthDiameter))
                return BadRequest("Missing required parameters: stoneType, stoneShape, and lengthDiameter are required.");
            if (growingType == "Lab - HPHT  CVD") { growingType = "Lab - HPHT / CVD"; }

            var weight = await _masterDataRepository.GetPerStoneWeight(stoneType, growingType, stoneShape, lengthDiameter);

            if (weight == null)
                return NotFound();

            return Ok(new { perStoneWeight = weight });
        }
        // GET api/sku/stonecostpercarat?stoneType=...&growingType=...&stoneShape=...&lengthDiameter=...
        [HttpGet]
        [Route("stonecostpercarat")]
        public async Task<IHttpActionResult> GetStoneCostPerCarat([FromUri] string vendor, string stoneType, [FromUri] string growingType, [FromUri] string stoneShape, [FromUri] string lengthDiameter, [FromUri] string stoneQuality)
        {
            if (string.IsNullOrWhiteSpace(stoneType) || string.IsNullOrWhiteSpace(stoneShape) || string.IsNullOrWhiteSpace(lengthDiameter) || string.IsNullOrWhiteSpace(stoneQuality))
                return BadRequest("Missing required parameters: stoneType, stoneShape, lengthDiameter, stoneQuality.");
            if (growingType == "Lab - HPHT  CVD") { growingType = "Lab - HPHT / CVD"; }
            var cost = await _masterDataRepository.GetStoneCostPerCarat(vendor, stoneType, growingType, stoneShape, lengthDiameter, stoneQuality);

            if (cost == null)
                return NotFound();

            return Ok(new { stoneCostPerCarat = cost });
        }
        // GET api/sku/settingtype?settingVendor=...
        [HttpGet]
        [Route("settingtype")]
        public async Task<IHttpActionResult> GetShapesByVendor([FromUri] string settingVendor)
        {
            if (string.IsNullOrWhiteSpace(settingVendor))
                return BadRequest("SettingVendor is required.");

            var shapes = _masterDataRepository.GetDropdownFromDb("SettingType", settingVendor);

            if (shapes == null || !shapes.Any())
                return NotFound();

            await Task.Delay(0);
            return Ok(shapes);
        }
        // GET api/sku/settingcostperstone?vendor=...&settingType=...&perStoneWt=...
        [HttpGet]
        [Route("settingcostperstone")]
        public async Task<IHttpActionResult> GetCostPerStone([FromUri] string vendor, [FromUri] string settingType, [FromUri] decimal perStoneWt, [FromUri] string shape, [FromUri] string category, [FromUri] string subCategory)
        {
            if (string.IsNullOrWhiteSpace(vendor) || string.IsNullOrWhiteSpace(settingType) || string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(subCategory))
                return BadRequest("Category, Subcategory, Vendor and SettingType are required.");

            var cost = await _masterDataRepository.GetSettingCostPerStone(vendor, settingType, perStoneWt,shape, category, subCategory);

            if (cost == null)
                return NotFound();

            return Ok(new { costPerStone = cost });
        }


       



        // ============================================
        // POST: api/sku/savesku
        // ============================================
        [HttpPost]
        [Route("savesku")]
        public IHttpActionResult SaveSku([FromBody] SkuModuleDto model)
        {
            if (model == null)
                return BadRequest("Invalid SKU payload");

            try
            {
                long skuId = _skuRepository.SaveSkuModule(model);

                return Ok(new
                {
                    SKUId = skuId,
                    Message = "SKU saved successfully"
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetList(string company,string skuPrefix = null,string category = null,string subcategory = null,string collection = null,int page = 1,int pageSize = 10)
        {
            //if ((string.IsNullOrWhiteSpace(skuPrefix) || skuPrefix.Length < 5) && category == null)
            //    return Ok();
            var result = await _skuRepository.GetPagedAsync(company, skuPrefix, category, subcategory, collection, page, pageSize);

            return Ok(result);
        }
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var sku = await _skuRepository.GetByIdAsync(id);
            if (sku == null)
                return NotFound();

            return Ok(sku);

        }
        [HttpGet]
        [Route("exists/{skuNumber}")]
        public async Task<IHttpActionResult> CheckSkuExists(string skuNumber)
        {
            bool exists = await _skuRepository.ExistsAsync(skuNumber);
            return Ok(new { Exists = exists });
        }



    }
}
