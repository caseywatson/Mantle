using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoCopyService
    {
        void CopyPhoto(PhotoMetadata photoMetadata, string photoSource, string photoDestination);
    }
}