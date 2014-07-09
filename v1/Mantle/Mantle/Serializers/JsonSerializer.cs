using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Serializers
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public T Deserialize(string source)
        {
            source.Require("source");
            return source.FromJson<T>();
        }

        public string Serialize(T source)
        {
            return source.ToJson();
        }
    }
}