namespace Mantle.Configuration
{
    public interface IConfigurable
    {
        void Configure(IConfigurationMetadata metadata);
    }
}