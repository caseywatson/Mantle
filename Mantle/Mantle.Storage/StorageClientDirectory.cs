using System;
using System.Collections.Generic;

namespace Mantle.Storage
{
    public class StorageClientDirectory : Dictionary<string, IStorageClient>, IStorageClientDirectory
    {
        public StorageClientDirectory(IStorageClient[] storageClients)
        {
            if (storageClients == null)
                throw new ArgumentNullException("storageClients");

            Load(storageClients);
        }

        private void Load(IStorageClient[] storageClients)
        {
            foreach (IStorageClient storageClient in storageClients)
            {
                if (ContainsKey(storageClient.Name))
                    throw new StorageException(
                        String.Format("This directory already contains a storage client named [{0}].",
                                      storageClient.Name));

                storageClient.Validate();

                Add(storageClient.Name, storageClient);
            }
        }
    }
}