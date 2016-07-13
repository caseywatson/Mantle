using System;

namespace Mantle.PhotoGallery.Web.Models
{
    public class PhotoViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime PhotoDateUtc { get; set; }
    }
}