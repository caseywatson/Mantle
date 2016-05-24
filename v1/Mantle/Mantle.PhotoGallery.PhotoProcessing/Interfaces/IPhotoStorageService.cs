using System.IO;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoStorageService
    {
        MemoryStream LoadPhoto(string photoId);
        void SavePhoto(string photoId, MemoryStream photoStream);
    }
}