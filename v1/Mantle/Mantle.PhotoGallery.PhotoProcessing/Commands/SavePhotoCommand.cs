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

        public SavePhotoCommand(Photo photo)
            : this()
        {
            photo.Require(nameof(photo));

            Photo = photo;
        }

        public Photo Photo { get; set; }

        public string Id { get; set; }
    }
}