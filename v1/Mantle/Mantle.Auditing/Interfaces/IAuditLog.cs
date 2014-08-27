namespace Mantle.Auditing.Interfaces
{
    public interface IAuditLog<T>
    {
        string GetEventId(T auditEvent);
        string GetEventPartitionId(T auditEvent);

        void Record(T auditEvent);
    }
}