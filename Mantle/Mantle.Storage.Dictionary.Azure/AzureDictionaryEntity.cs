using Microsoft.WindowsAzure.Storage.Table;

namespace Mantle.Storage.Dictionary.Azure
{
    public class AzureDictionaryEntity : TableEntity
    {
        public byte[] Data { get; set; }
    }
}