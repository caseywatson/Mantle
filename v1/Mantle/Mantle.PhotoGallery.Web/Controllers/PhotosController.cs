using System.Linq;
using System.Net;
using System.Web.Mvc;
using Mantle.BlobStorage.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Constants;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Mantle.PhotoGallery.Web.Models;
using Microsoft.AspNet.Identity;

namespace Mantle.PhotoGallery.Web.Controllers
{
    public class PhotosController : Controller
    {
        private readonly IPublisherChannel<MessageEnvelope> copyImageCommandChannel;
        private readonly IPhotoMetadataRepository photoMetadataRepository;
        private readonly IBlobStorageClient photoStorageClient;
        private readonly IPublisherChannel<MessageEnvelope> saveImageCommandChannel;
        private readonly IBlobStorageClient thumbnailStorageClient;

        private string photoSource;

        public PhotosController(IPhotoMetadataRepository photoMetadataRepository,
                               IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory,
                               IDirectory<IBlobStorageClient> storageClientDirectory)
        {
            this.photoMetadataRepository = photoMetadataRepository;

            copyImageCommandChannel = publisherChannelDirectory[ChannelNames.CopyImageCommandChannel];
            saveImageCommandChannel = publisherChannelDirectory[ChannelNames.SaveImageCommandChannel];

            photoStorageClient = storageClientDirectory[BlobStorageClientNames.PhotoStorage];
            thumbnailStorageClient = storageClientDirectory[BlobStorageClientNames.ThumbnailStorage];
        }

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var photoMetadata = photoMetadataRepository.GetPhotoMetadata(id);

            if (photoMetadata == null)
                return HttpNotFound();

            var photo = photoStorageClient.DownloadBlob(id);

            if (photo == null)
                return HttpNotFound();

            return File(photo, photoMetadata.ContentType);
        }

        public ActionResult Thumbnail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var photoMetadata = photoMetadataRepository.GetPhotoMetadata(id);

            if (photoMetadata == null)
                return HttpNotFound();

            var thumbnail = thumbnailStorageClient.DownloadBlob(id);

            if (thumbnail == null)
                return HttpNotFound();

            return File(thumbnail, photoMetadata.ContentType);
        }

        [Authorize]
        public ActionResult My()
        {
            return View(photoMetadataRepository.GetAllPhotoMetadataByUser(User.Identity.GetUserId())
                            .OrderByDescending(pm => pm.PhotoDateUtc)
                            .Select(ToPhotoViewModel)
                            .ToList());
        }

        [Authorize]
        public ActionResult Upload()
        {
            return View(new PhotoUploadViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Upload(PhotoUploadViewModel viewModel)
        {
            if (ModelState.IsValid == false)
                return View(viewModel);

            var photoMetadata = ToPhotoMetadata(viewModel);

            photoStorageClient.UploadBlob(viewModel.Photo.InputStream, photoMetadata.Id);

            copyImageCommandChannel.Publish(new CopyPhoto(GetPhotoSource(), photoMetadata));
            saveImageCommandChannel.Publish(new SavePhoto(photoMetadata));

            return RedirectToAction("Index", "Home");
        }

        private PhotoMetadata ToPhotoMetadata(PhotoUploadViewModel viewModel)
        {
            return new PhotoMetadata
            {
                ContentType = viewModel.Photo.ContentType,
                Description = viewModel.Description,
                Title = viewModel.Title,
                UserId = User.Identity.GetUserId()
            };
        }

        private PhotoViewModel ToPhotoViewModel(PhotoMetadata photoMetadata)
        {
            return new PhotoViewModel
            {
                Description = photoMetadata.Description,
                Id = photoMetadata.Id,
                PhotoUrl = Url.Action("Index", new {id = photoMetadata.Id}),
                ThumbnailUrl = Url.Action("Thumbnail", new {id = photoMetadata.Id}),
                Title = photoMetadata.Title
            };
        }

        private string GetPhotoSource()
        {
            return (photoSource = ((photoSource) ??
                                   (MantleContext.Current.LoadedProfiles.Contains(PhotoSources.Aws)
                                       ? PhotoStorageClientNames.Aws.PhotoStorage
                                       : PhotoStorageClientNames.Azure.PhotoStorage)));
        }
    }
}