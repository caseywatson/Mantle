using System.IO;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoThumbnailService
    {
        MemoryStream GenerateThumbnail(MemoryStream originalImage);
    }
}