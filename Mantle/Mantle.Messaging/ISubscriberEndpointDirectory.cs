using System.Collections.Generic;

namespace Mantle.Messaging
{
    public interface ISubscriberEndpointDirectory : IDictionary<string, ISubscriberEndpoint>
    {
    }
}