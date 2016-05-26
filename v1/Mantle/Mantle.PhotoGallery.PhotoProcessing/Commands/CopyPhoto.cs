using System;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Commands
{
    public class CopyPhoto : ICommand
    {
        public CopyPhoto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public CopyPhoto(string photoSource, PhotoMetadata photoMetadata)
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