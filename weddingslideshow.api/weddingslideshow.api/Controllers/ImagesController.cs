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

        public async Task<PagedImageResponse> Get(string tag, string maxId)
        {
            var images = await service.LoadImages(tag, maxId);

            var imageContracts = images.Select(i => 
            {
                return ConvertImageMetadataToContract(i);
            });

            return new PagedImageResponse(imageContracts);
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