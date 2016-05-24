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

        public CopyPhotoCommand(string photoSource, Photo photo)
            : this()
        {
            photoSource.Require(nameof(photoSource));
            photo.Require(nameof(photo));

            PhotoSource = photoSource;
            Photo = photo;
        }

        public string PhotoSource { get; set; }
        public Photo Photo { get; set; }

        public string Id { get; set; }
    }
}