namespace Mantle.Azure
{
    public interface IAzureServiceBusConfiguration
    {
        string ConnectionString { get; set; }

        void Validate();
    }
}