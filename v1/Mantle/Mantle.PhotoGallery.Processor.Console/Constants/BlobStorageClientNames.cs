using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.PhotoGallery.Processor.Console.Constants
{
    public static class BlobStorageClientNames
    {
        public static class Aws
        {
            public const string PhotoStorage = "Aws.PhotoStorage";
            public const string ThumbnailStorage = "Aws.ThumbnailStorage";
        }

        public static class Azure
        {
            public const string PhotoStorage = "Azure.PhotoStorage";
            public const string ThumbnailStorage = "Azure.ThumbnailStorage";
        }
    }
}
