using System;

namespace Mantle.WindowsServiceBus
{
    public interface IWindowsServiceBusConfiguration
    {
        string ConnectionString { get; set; }

        void Validate();
    }
}
