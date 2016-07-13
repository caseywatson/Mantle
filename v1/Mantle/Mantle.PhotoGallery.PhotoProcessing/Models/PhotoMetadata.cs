using System;

namespace Mantle.PhotoGallery.PhotoProcessing.Models
{
    public class PhotoMetadata
    {
        public PhotoMetadata()
        {
            Id = Guid.NewGuid().ToString();
            PhotoDateUtc = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string ContentType { get; set; }
        public DateTime PhotoDateUtc { get; set; }
    }
}