using System.IO;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoThumbnailService
    {
        Stream GenerateThumbnail(Stream originalImageStream);
    }
}