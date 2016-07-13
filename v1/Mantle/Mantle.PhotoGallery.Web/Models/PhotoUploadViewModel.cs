using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Mantle.PhotoGallery.Web.Models
{
    public class PhotoUploadViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        public HttpPostedFileBase Photo { get; set; }
    }
}