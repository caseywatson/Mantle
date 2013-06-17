using System;

namespace Mantle.Messaging
{
    public abstract class Endpoint
    {
        public string Name { get; set; }

        public virtual void Validate()
        {
            if (String.IsNullOrEmpty(Name))
                throw new MessagingException("Endpoint name is required.");
        }
    }
}