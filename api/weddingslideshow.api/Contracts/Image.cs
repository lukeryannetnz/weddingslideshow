namespace weddingslideshow.api.Contracts
{
    using System;

    /// <summary>
    /// Public API contract for an Image.
    /// </summary>
    public class Image
    {
        public string Id{get;set;}

        public Uri Location {get; set;}
  }
}