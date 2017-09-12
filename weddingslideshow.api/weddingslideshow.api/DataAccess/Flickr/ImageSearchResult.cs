namespace weddingslideshow.api.DataAccess
{
    using System;
    using System.Collections.Generic;

    public class Photo
    {
        public string id { get; set; }
        public string url_o { get; set; }
    }

    public class Photos
    {
        public int page { get; set; }
        public string pages { get; set; }
        public int perpage { get; set; }
        public string total { get; set; }
        public IList<Photo> photo { get; set; }
    }

    public class ImageSearchResult
    {
        public Photos photos { get; set; }
    }
}