using System.Collections.Generic;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoMetadataRepository
    {
        bool DeletePhotoMetadata(PhotoMetadata photoMetadata);
        void InsertOrUpdatePhotoMetadata(PhotoMetadata photoMetadata);
        PhotoMetadata GetPhotoMetadata(string photoId);
        IEnumerable<PhotoMetadata> GetAllPhotoMetadata();
        IEnumerable<PhotoMetadata> GetAllPhotoMetadataByUser(string userId);
    }
}