using System;

namespace Mantle.PhotoGallery.PhotoProcessing.Models
{
    public class Photo
    {
        public Photo()
        {
            Id = Guid.NewGuid().ToString();
            PhotoDateUtc = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PhotoDateUtc { get; set; }
    }
}