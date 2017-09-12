namespace weddingslideshow.api.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    
    using weddingslideshow.api.DataAccess;

    /// <summary> 
    /// Communicates with the Flickr API.
    /// <seealso href="https://www.flickr.com/services/api/">The Flicker API documentation.</seealso>
    /// </summary>
    public interface IImageService
    {
        Task<IEnumerable<ImageMetadata>> LoadImages(string query, string fromId);
    }
}