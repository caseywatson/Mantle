namespace Mantle.Configuration
{
    public interface IConfigurationProvider
    {
        IConfiguration Load();
    }
}