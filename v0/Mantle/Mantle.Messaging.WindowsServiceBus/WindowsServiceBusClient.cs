using System;
using Mantle.WindowsServiceBus;

using Microsoft.ServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public abstract class WindowsServiceBusClient
    {
        protected readonly NamespaceManager NsManager;

        protected WindowsServiceBusClient(IWindowsServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            try
            {
                NsManager = NamespaceManager.CreateFromConnectionString(sbConfiguration.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access the specified Windows service bus. See inner exception for more details.",
                    ex);
            }
        }
    }
}