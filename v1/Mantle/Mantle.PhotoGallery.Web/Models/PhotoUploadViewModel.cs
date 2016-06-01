using System.ComponentModel.DataAnnotations;
using System.Web;
using Mantle.PhotoGallery.Web.Attributes;

namespace Mantle.PhotoGallery.Web.Models
{
    public class PhotoUploadViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxUploadSize((5*1024*1024), ErrorMessage = "Uploaded photo must be less than 5 MB in size.")]
        [MustBeImage]
        [Required]
        public HttpPostedFileBase Photo { get; set; }
    }
}