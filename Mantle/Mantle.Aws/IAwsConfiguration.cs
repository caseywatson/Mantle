using Amazon;

namespace Mantle.Aws
{
    public interface IAwsConfiguration
    {
        string AccessKey { get; set; }
        string SecretKey { get; set; }

        void Validate();
    }
}