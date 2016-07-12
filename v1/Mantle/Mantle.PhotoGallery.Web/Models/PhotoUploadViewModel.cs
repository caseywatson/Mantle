using System.ComponentModel.DataAnnotations;
using System.Web;
using Mantle.PhotoGallery.Web.Attributes;

namespace Mantle.PhotoGallery.Web.Models
{
    public class PhotoUploadViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxUploadSize((5*1024*1024), ErrorMessage = "Photo must be 5 mb or less.")]
        [MustBeImage(ErrorMessage = "Photo must be bmp, gif, jpg/jpeg or png.")]
        [Required(ErrorMessage = "Photo is required.")]
        public HttpPostedFileBase Photo { get; set; }
    }
}