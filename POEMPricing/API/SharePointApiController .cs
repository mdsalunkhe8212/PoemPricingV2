using POEM.Services.Interface;
using POEM.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POEMPricing.API
{
    [RoutePrefix("api/sharepoint")]
    public class SharePointApiController : ApiController
    {
        private readonly ISharePointRepository _repository;

        public SharePointApiController()
        {
            _repository = new SharePointRepository(); // swap with DI below
        }

        // Constructor for DI
        public SharePointApiController(ISharePointRepository repository)
        {
            _repository = repository;
        }

        // GET api/sharepoint/image?filePath=/Shared Documents/Images/photo.jpg
        [HttpGet]
        [Route("image")]
        public async Task<IHttpActionResult> GetImage(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return BadRequest("filePath is required");

                var image = await _repository.GetImageAsync(filePath);

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(image.Content)
                };
                response.Content.Headers.ContentType =
                    new MediaTypeHeaderValue(image.MimeType);

                return ResponseMessage(response);
            }
           
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //// GET api/sharepoint/images?folderPath=/Shared Documents/Images
        //[HttpGet]
        //[Route("images")]
        //public async Task<IHttpActionResult> GetImageList(string folderPath)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(folderPath))
        //            return BadRequest("folderPath is required");

        //        var files = await _repository.GetImageListAsync(folderPath);
        //        return Ok(files); // returns JSON array of paths
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}