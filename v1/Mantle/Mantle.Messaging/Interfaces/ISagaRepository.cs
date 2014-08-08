using System.Collections.Generic;

namespace Mantle.Messaging.Interfaces
{
    public interface ISagaRepository<TEnvelope>
    {
        void AddMessageEnvelope(TEnvelope messageEnvelope);

        void DeleteMessages(string correlationId);

        IEnumerable<TEnvelope> LoadMessageEnvelopes(string correlationId);
        IEnumerable<TMessage> LoadMessages<TMessage>(string correlationId);
    }
}