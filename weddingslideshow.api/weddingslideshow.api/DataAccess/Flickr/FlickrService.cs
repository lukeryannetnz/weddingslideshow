using System;
using Microsoft.Extensions.Configuration;

namespace weddingslideshow.api.DataAccess
{
    using System;
    using System.Collections.Generic;

    using weddingslideshow.api.DataAccess;

    public class FlickrService : IImageService
    {
        public FlickrService(IConfiguration config)
        {
            
        }

        public IEnumerable<ImageMetadata> LoadImages(string query, string fromId)
        {
            return new [] { new ImageMetadata()
                {
                    ImageLocation = new Uri("http://www.rewildthyself.com/wp-content/uploads/2015/03/poop.jpeg"),
                    Id = "1"
                }
            };
        }
    }
}