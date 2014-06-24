using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class Message
    {
        public byte[] Body { get; set; }
        public string BodyTypeToken { get; set; }
        public string CorrelationId { get; set; }
        public int? DeliveryCount { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public int TimeToLive { get; set; }
    }
}