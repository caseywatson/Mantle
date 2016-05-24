using System;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Commands
{
    public class SavePhotoCommand : ICommand
    {
        public SavePhotoCommand()
        {
            Id = Guid.NewGuid().ToString();
        }

        public SavePhotoCommand(PhotoMetadata photoMetadata)
            : this()
        {
            photoMetadata.Require(nameof(photoMetadata));

            PhotoMetadata = photoMetadata;
        }

        public PhotoMetadata PhotoMetadata { get; set; }

        public string Id { get; set; }
    }
}