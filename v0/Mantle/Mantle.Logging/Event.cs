using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Mantle.Logging
{
    [DataContract(Namespace = "http://mantle/logging/1")]
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            DatePosted = DateTime.UtcNow;
        }

        public Event(string body, Severity severity, string source = null, IEnumerable<string> tags = null)
            : this()
        {
            if (String.IsNullOrEmpty(body))
                throw new ArgumentException("Body is required.", "body");

            Body = body;
            Severity = severity;

            if (source != null)
                Source = source;

            if (tags != null)
                Tags = tags.ToArray();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string[] Tags { get; set; }

        [DataMember]
        public DateTime DatePosted { get; set; }

        [DataMember]
        public Severity Severity { get; set; }
    }
}