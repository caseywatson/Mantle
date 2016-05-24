using System;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Commands
{
    public class CopyPhotoCommand : ICommand
    {
        public CopyPhotoCommand()
        {
            Id = Guid.NewGuid().ToString();
        }

        public CopyPhotoCommand(string photoSource, PhotoMetadata photoMetadata)
            : this()
        {
            photoSource.Require(nameof(photoSource));
            photoMetadata.Require(nameof(photoMetadata));

            PhotoSource = photoSource;
            PhotoMetadata = photoMetadata;
        }

        public string PhotoSource { get; set; }
        public PhotoMetadata PhotoMetadata { get; set; }

        public string Id { get; set; }
    }
}