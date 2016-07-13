using Mantle.Configuration.Attributes;
using Mantle.PhotoGallery.Web.Enumerations;
using Mantle.PhotoGallery.Web.Interfaces;

namespace Mantle.PhotoGallery.Web.Metadata
{
    public class DeploymentMetadata : IDeploymentMetadata
    {
        [Configurable(IsRequired = true)]
        public string Name { get; set; }

        [Configurable(IsRequired = true)]
        public Platform Platform { get; set; }
    }
}