using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        /*------------------------------------------------------------------*/
        public ImagesController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadAsync([FromForm] ImageUploadDto imageUploadDto)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _imageManager.UploadAsync(imageUploadDto, basePath, schema, host);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
    }
}
