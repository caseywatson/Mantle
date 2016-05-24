using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Interfaces
{
    public interface IPhotoCopyService
    {
        void CopyPhoto(Photo photo, string photoSource);
    }
}