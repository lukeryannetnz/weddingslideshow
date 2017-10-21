namespace weddingslideshow.api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Mvc;

    using weddingslideshow.api.DataAccess;
    using weddingslideshow.api.Contracts;

    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageService service;

        public ImagesController(IImageService imageService)
        {
            service = imageService;
        }

        public async Task<IActionResult> Get(string tag, string maxId, int? page)
        {
            if (!string.IsNullOrWhiteSpace(maxId) && page.HasValue)
            {
                return StatusCode(400, "Either maxId or page can be specified, not both.");
            }

            IEnumerable<ImageMetadata> images;

            if (!string.IsNullOrWhiteSpace(maxId))
            {
                images = await service.LoadImages(tag, maxId);
            }
            else if (page.HasValue)
            {
                images = await service.LoadImages(tag, page.Value);
            }
            else
            {
                images = await service.LoadImages(tag);
            }

            var imageContracts = images.Select(i =>
            {
                return ConvertImageMetadataToContract(i);
            });

            return new ObjectResult(new PagedImageResponse(imageContracts));
        }

        private Image ConvertImageMetadataToContract(ImageMetadata metadata)
        {
            return new Image
            {
                Id = metadata.Id,
                Location = metadata.ImageLocation
            };
        }
    }
}