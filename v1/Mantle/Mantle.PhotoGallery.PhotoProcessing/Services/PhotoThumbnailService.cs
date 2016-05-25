using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;

namespace Mantle.PhotoGallery.PhotoProcessing.Services
{
    public class PhotoThumbnailService : IPhotoThumbnailService
    {
        public PhotoThumbnailService()
        {
            MaxThumbnailHeight = 100;
            MaxThumbnailWidth = 100;
        }

        [Configurable]
        public int MaxThumbnailHeight { get; set; }

        [Configurable]
        public int MaxThumbnailWidth { get; set; }

        public Stream GenerateThumbnail(Stream originalImageStream)
        {
            originalImageStream.Require(nameof(originalImageStream));

            originalImageStream.TryToRewind();

            var originalImage = Image.FromStream(originalImageStream);
            var thumbnailSize = CalculateThumbnailSize(originalImage);
            var thumbnailRectangle = new Rectangle(new Point(), thumbnailSize);
            var thumbnailImage = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);

            thumbnailImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (var thumbnailGraphics = Graphics.FromImage(thumbnailImage))
            {
                thumbnailGraphics.CompositingMode = CompositingMode.SourceCopy;
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    thumbnailGraphics.DrawImage(originalImage, thumbnailRectangle, 0, 0, originalImage.Width,
                                                originalImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var thumbnailImageStream = new MemoryStream();

            thumbnailImage.Save(thumbnailImageStream, originalImage.RawFormat);
            thumbnailImageStream.FlushAsync().Wait();
            thumbnailImageStream.Position = 0;

            return thumbnailImageStream;
        }

        private Size CalculateThumbnailSize(Image originalImage)
        {
            var size = new Size(originalImage.Width, originalImage.Height);

            if ((originalImage.Height > MaxThumbnailHeight) || (originalImage.Width > MaxThumbnailWidth))
            {
                if (originalImage.Height >= originalImage.Width)
                {
                    size.Height = Math.Min(MaxThumbnailHeight, originalImage.Height);
                    size.Width = (int) (originalImage.Width*(((double) (size.Height))/originalImage.Height));

                    if (size.Width > MaxThumbnailWidth)
                    {
                        size.Height = (int) (size.Height*(((double) (MaxThumbnailWidth))/size.Width));
                        size.Width = MaxThumbnailWidth;
                    }
                }
                else
                {
                    size.Width = Math.Min(MaxThumbnailWidth, originalImage.Width);
                    size.Height = (int) (originalImage.Height*(((double) (size.Width))/originalImage.Width));

                    if (size.Height > MaxThumbnailHeight)
                    {
                        size.Width = (int) (size.Width*(((double) (MaxThumbnailHeight))/size.Height));
                        size.Height = MaxThumbnailHeight;
                    }
                }
            }

            return size;
        }
    }
}