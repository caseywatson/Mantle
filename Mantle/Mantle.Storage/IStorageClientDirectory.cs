using System.Collections.Generic;

namespace Mantle.Storage
{
    public interface IStorageClientDirectory : IDictionary<string, IStorageClient>
    {
    }
}