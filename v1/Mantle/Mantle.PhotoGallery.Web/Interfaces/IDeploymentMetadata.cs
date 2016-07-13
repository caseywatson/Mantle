using Mantle.PhotoGallery.Web.Enumerations;

namespace Mantle.PhotoGallery.Web.Interfaces
{
    public interface IDeploymentMetadata
    {
        string Name { get; }
        Platform Platform { get; }
    }
}