using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mantle.Extensions;

namespace Mantle.PhotoGallery.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MustBeImageAttribute : ValidationAttribute
    {
        private readonly string[] mimeTypes;

        public MustBeImageAttribute()
        {
            mimeTypes = new[]
            {
                "image/bmp",
                "image/gif",
                "image/jpeg",
                "image/jpg",
                "image/png"
            };

            ErrorMessage =
                "{DisplayName} must be an image. Acceptable formats are bitmap [bmp], [gif], [jpeg/jpg] and [png].";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpPostedFile = (value as HttpPostedFileBase);

            if (httpPostedFile == null)
                throw new ArgumentException($"[{nameof(value)}] must be [HttpPostedFileBase].", nameof(value));

            if (mimeTypes.Contains(httpPostedFile.ContentType.ToLower()))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage.Merge(new {validationContext.DisplayName}));
        }
    }
}