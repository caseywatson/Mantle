using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Serializers
{
    public class XmlSerializer<T> : ISerializer<T>
    {
        public T Deserialize(string source)
        {
            source.Require("source");
            return source.FromXml<T>();
        }

        public string Serialize(T source)
        {
            return source.ToXml();
        }
    }
}