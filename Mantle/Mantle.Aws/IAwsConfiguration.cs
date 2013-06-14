using Amazon;

namespace Mantle.Aws
{
    public interface IAwsConfiguration
    {
        RegionEndpoint Region { get; set; }

        string AccessKey { get; set; }
        string SecretKey { get; set; }
    }
}