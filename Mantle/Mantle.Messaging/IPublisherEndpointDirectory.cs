using System.Collections.Generic;

namespace Mantle.Messaging
{
    public interface IPublisherEndpointDirectory : IDictionary<string, IPublisherEndpoint>
    {
    }
}