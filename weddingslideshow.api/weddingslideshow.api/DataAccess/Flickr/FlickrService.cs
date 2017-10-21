namespace weddingslideshow.api.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    using weddingslideshow.api.DataAccess;

    /// <summary> 
    /// Communicates with the Flickr API.
    /// <seealso href="https://www.flickr.com/services/api/">The Flicker API documentation.</seealso>
    /// </summary>
    public class FlickrService : IImageService
    {
        private readonly string apiKey;
        private readonly string apiSecret;
        private readonly Uri apiUri;

        public FlickrService(IConfiguration config)
        {
            apiKey = config["FlickrAPI:ApiKey"];
            apiSecret = config["FlickrAPI:ApiSecret"];
            apiUri = new Uri(config["FlickrAPI:Uri"]);
        }

        public async Task<IEnumerable<ImageMetadata>> LoadImages(string query, string fromId)
        {
            throw new Exception("Paging images via watermark is not supported by the Flickr API.");
        }

        public async Task<IEnumerable<ImageMetadata>> LoadImages(string query, int page)
        {
            var uri = CreateUri(query, page);
            var result = await LoadImagesFromApi(uri);

            return ConvertApiResponse(result);
        }

        public async Task<IEnumerable<ImageMetadata>> LoadImages(string query)
        {
            var uri = CreateUri(query);
            var result = await LoadImagesFromApi(uri);

            return ConvertApiResponse(result);
        }

        private string CreateUri(string tag)
        {
            return $"{apiUri}?method=flickr.photos.search&api_key={apiKey}&tags={tag}&extras=url_o&per_page=100&format=json&nojsoncallback=1";
        }

        private string CreateUri(string tag, int page)
        {
            return $"{apiUri}?method=flickr.photos.search&api_key={apiKey}&tags={tag}&extras=url_o&per_page=100&page={page}&format=json&nojsoncallback=1";
        }

        private async Task<ImageSearchResult> LoadImagesFromApi(string searchUri)
        {
            var client = new HttpClient();
            var stringTask = client.GetStringAsync(searchUri);
            var json = await stringTask;

            return JsonConvert.DeserializeObject<ImageSearchResult>(json);
        }

        private IEnumerable<ImageMetadata> ConvertApiResponse(ImageSearchResult result)
        {
            foreach (var p in result.photos.photo)
            {
                if (Uri.IsWellFormedUriString(p.url_o, UriKind.Absolute))
                {
                    yield return new ImageMetadata()
                    {
                        Id = p.id,
                        ImageLocation = new Uri(p.url_o)
                    };
                }
            }
        }
    }
}