using System;
using Mantle.Auditing.Interfaces;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.Auditing.Repositories
{
    public class DictionaryStorageAuditLog<T> : IAuditLog<T>
        where T : class, new()
    {
        private readonly IDictionaryStorageClient<T> dictionaryStorageClient;

        public DictionaryStorageAuditLog(IDictionaryStorageClient<T> dictionaryStorageClient)
        {
            this.dictionaryStorageClient = dictionaryStorageClient;
        }

        public void Record(T auditEvent)
        {
            auditEvent.Require("auditEvent");

            dictionaryStorageClient.InsertOrUpdateEntity(auditEvent,
                                                         GetEventId(auditEvent),
                                                         GetEventPartitionId(auditEvent));
        }

        protected virtual string GetEventId(T auditEvent)
        {
            return (Guid.NewGuid().ToString());
        }

        protected virtual string GetEventPartitionId(T auditEvent)
        {
            return (typeof (T).Name.ToLower());
        }
    }
}