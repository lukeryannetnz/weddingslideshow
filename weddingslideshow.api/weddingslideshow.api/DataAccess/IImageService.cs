namespace weddingslideshow.api.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    
    using weddingslideshow.api.DataAccess;

    /// <summary> 
    /// Loads images from a server.
    /// </summary>
    public interface IImageService
    {
        Task<IEnumerable<ImageMetadata>> LoadImages(string query, string fromId);


        Task<IEnumerable<ImageMetadata>> LoadImages(string query, int page);


        Task<IEnumerable<ImageMetadata>> LoadImages(string query);
    }
}