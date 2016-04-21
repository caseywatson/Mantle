using Amazon;

namespace Mantle.Aws.Interfaces
{
    public interface IAwsRegionEndpoints
    {
        RegionEndpoint GetRegionEndpointByName(string regionName);
    }
}