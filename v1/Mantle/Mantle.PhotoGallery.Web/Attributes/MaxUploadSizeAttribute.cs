using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mantle.Extensions;

namespace Mantle.PhotoGallery.Web.Attributes
{
    public class MaxUploadSizeAttribute : ValidationAttribute
    {
        public MaxUploadSizeAttribute(int maxUploadSizeInBytes)
        {
            MaxUploadSizeInBytes = maxUploadSizeInBytes;

            ErrorMessage = "{DisplayName} is too large. Maximum upload size is [{MaxUploadSizeInBytes}] byte(s).";
        }

        public int MaxUploadSizeInBytes { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpPostedFile = (value as HttpPostedFileBase);

            if (httpPostedFile == null)
                throw new ArgumentException($"[{nameof(value)}] must be [HttpPostedFileBase].", nameof(value));

            if (httpPostedFile.ContentLength > MaxUploadSizeInBytes)
            {
                var parameters = new
                {
                    validationContext.DisplayName,
                    MaxUploadSizeInBytes
                };

                return new ValidationResult(ErrorMessage.Merge(parameters));
            }

            return ValidationResult.Success;
        }
    }
}
