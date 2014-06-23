using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class Message
    {
        public byte[] Body { get; set; }
        public string BodyTypeName { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public int TimeToLive { get; set; }
    }
}