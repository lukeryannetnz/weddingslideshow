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
            var result = await LoadImagesFromApi(query);

            return ConvertApiResponse(result);
        }

        private async Task<ImageSearchResult> LoadImagesFromApi(string tag)
        {
            var searchUri = $"{apiUri}?method=flickr.photos.search&api_key={apiKey}&tags={tag}&extras=url_o&format=json&nojsoncallback=1&per_page=20";

            var client = new HttpClient();
            var stringTask = client.GetStringAsync(searchUri);
            var json = await stringTask;

            return JsonConvert.DeserializeObject<ImageSearchResult>(json);
        }

        private IEnumerable<ImageMetadata> ConvertApiResponse(ImageSearchResult result)
        {
            foreach(var p in result.photos.photo)
            {
                if(Uri.IsWellFormedUriString(p.url_o, UriKind.Absolute))
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