using System.Runtime.Serialization;

namespace Mantle.Logging
{
    [DataContract(Namespace = "http://mantle/logging/1")]
    public enum Severity : byte
    {
        [EnumMember] Undefined = 0,
        [EnumMember] Debug,
        [EnumMember] Information,
        [EnumMember] Warning,
        [EnumMember] Error,
        [EnumMember] Fatal
    }
}