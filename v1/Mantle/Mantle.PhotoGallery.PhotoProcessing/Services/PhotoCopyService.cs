using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Services
{
    public class PhotoCopyService : IPhotoCopyService
    {
        private readonly IDirectory<IPhotoStorageService> photoStorageServices;

        public PhotoCopyService(IDirectory<IPhotoStorageService> photoStorageServices)
        {
            this.photoStorageServices = photoStorageServices;
        }

        public void CopyPhoto(PhotoMetadata photoMetadata, string photoSource, string photoDestination)
        {
            photoMetadata.Require(nameof(photoMetadata));
            photoSource.Require(nameof(photoSource));
            photoDestination.Require(nameof(photoDestination));

            var sourcePhotoStorageService = photoStorageServices[photoSource];
            var destinationPhotoStorageService = photoStorageServices[photoDestination];
            var sourcePhotoStream = sourcePhotoStorageService.LoadPhoto(photoMetadata.Id);

            sourcePhotoStream.TryToRewind();

            destinationPhotoStorageService.SavePhoto(photoMetadata.Id, sourcePhotoStream);
        }
    }
}