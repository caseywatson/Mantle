using Mantle.Auditing.Interfaces;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Auditing.Subscribers
{
    public class AuditSubscriber<T> : ISubscriber<T>
        where T : class
    {
        private readonly IAuditLog<T> auditRepository;

        public AuditSubscriber(IAuditLog<T> auditRepository)
        {
            this.auditRepository = auditRepository;
        }

        public virtual void HandleMessage(IMessageContext<T> messageContext)
        {
            messageContext.Require("messageContext");
            auditRepository.Record(messageContext.Message);
        }
    }
}