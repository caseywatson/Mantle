using System;

namespace Mantle.Storage
{
    public abstract class StorageClient
    {
        public string Name { get; set; }

        public virtual void Validate()
        {
            if (String.IsNullOrEmpty(Name))
                throw new StorageException("Storage name is required.");
        }
    }
}