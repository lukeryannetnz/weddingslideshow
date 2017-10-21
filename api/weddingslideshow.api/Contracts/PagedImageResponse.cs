namespace weddingslideshow.api.Contracts
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class PagedImageResponse
    {
        public PagedImageResponse()
        {
            this.Data = new List<Image>();
            this.Pagination = new Pagination();
        }

        public PagedImageResponse(IEnumerable<Image> images)
        {
            this.Data = images;
            this.Pagination = new Pagination();

            this.Pagination.NextMaxId = images.Min(i => i.Id);
        }

        public IEnumerable<Image> Data {get;set;}

        public Pagination Pagination {get;set;}
    }

    /// <summary>
    /// 
    /// </summary>
    public class Pagination
    {
        public string NextMaxId{get;set;}

    }
}