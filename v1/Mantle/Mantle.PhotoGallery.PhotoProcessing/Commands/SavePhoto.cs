using System;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Commands
{
    public class SavePhoto : ICommand
    {
        public SavePhoto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public SavePhoto(PhotoMetadata photoMetadata)
            : this()
        {
            photoMetadata.Require(nameof(photoMetadata));

            PhotoMetadata = photoMetadata;
        }

        public PhotoMetadata PhotoMetadata { get; set; }

        public string Id { get; set; }
    }
}