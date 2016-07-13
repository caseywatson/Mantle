using System.Collections.Generic;
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
using Mantle.PhotoGallery.Web.Enumerations;
using Mantle.PhotoGallery.Web.Interfaces;
using Mantle.PhotoGallery.Web.Mantle.Constants;
using Mantle.PhotoGallery.Web.Models;
using Microsoft.AspNet.Identity;

namespace Mantle.PhotoGallery.Web.Controllers
{
    public class PhotosController : Controller
    {
        private const double MaxPhotoUploadSize = (5 * 1024 * 1024);

        private readonly string[] allowedPhotoUploadMimeTypes =
        {
            "image/bmp",
            "image/gif",
            "image/jpeg",
            "image/jpg",
            "image/png"
        };

        private readonly IPublisherChannel<MessageEnvelope> copyImageCommandChannel;
        private readonly IDeploymentMetadata deploymentMetadata;
        private readonly IPhotoMetadataRepository photoMetadataRepository;

        private readonly Dictionary<Platform, string> photoSourceDictionary = new Dictionary<Platform, string>
        {
            [Platform.Aws] = PhotoStorageClientNames.Aws.PhotoStorage,
            [Platform.Azure] = PhotoStorageClientNames.Azure.PhotoStorage
        };

        private readonly IBlobStorageClient photoStorageClient;
        private readonly IPublisherChannel<MessageEnvelope> saveImageCommandChannel;
        private readonly IBlobStorageClient thumbnailStorageClient;

        public PhotosController(IDeploymentMetadata deploymentMetadata,
                                IPhotoMetadataRepository photoMetadataRepository,
                                IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory,
                                IDirectory<IBlobStorageClient> storageClientDirectory)
        {
            this.deploymentMetadata = deploymentMetadata;
            this.photoMetadataRepository = photoMetadataRepository;

            copyImageCommandChannel = publisherChannelDirectory[ChannelNames.CopyImageCommandChannel];
            saveImageCommandChannel = publisherChannelDirectory[ChannelNames.SaveImageCommandChannel];

            photoStorageClient = storageClientDirectory[BlobStorageClientNames.PhotoStorage];
            thumbnailStorageClient = storageClientDirectory[BlobStorageClientNames.ThumbnailStorage];
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult Photo(string id)
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


        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
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

        public ActionResult All()
        {
            return View(photoMetadataRepository.GetAllPhotoMetadata()
                            .OrderByDescending(pm => pm.PhotoDateUtc)
                            .Select(ToPhotoViewModel)
                            .ToList());
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
            if (viewModel.Photo != null)
            {
                if (viewModel.Photo.ContentLength > MaxPhotoUploadSize)
                    ModelState.AddModelError("Photo", "Photo must be 5 mb or less in size.");

                if (allowedPhotoUploadMimeTypes.Contains(viewModel.Photo.ContentType.ToLower()) == false)
                    ModelState.AddModelError("Photo", "Photo type must be bmp, gif, jpeg or png.");
            }

            if (ModelState.IsValid == false)
                return View(viewModel);

            var photoMetadata = ToPhotoMetadata(viewModel);

            photoStorageClient.UploadBlob(viewModel.Photo.InputStream, photoMetadata.Id);

            copyImageCommandChannel.Publish(new CopyPhoto(photoSourceDictionary[deploymentMetadata.Platform],
                                                          photoMetadata));

            saveImageCommandChannel.Publish(new SavePhoto(photoMetadata));

            return RedirectToAction("My");
        }

        private PhotoMetadata ToPhotoMetadata(PhotoUploadViewModel viewModel)
        {
            var identity = User.Identity;

            return new PhotoMetadata
            {
                ContentType = viewModel.Photo.ContentType,
                Description = viewModel.Description,
                Source = deploymentMetadata.Name,
                Title = viewModel.Title,
                UserId = identity.GetUserId(),
                UserName = identity.GetUserName()
            };
        }

        private PhotoViewModel ToPhotoViewModel(PhotoMetadata photoMetadata)
        {
            return new PhotoViewModel
            {
                Description = photoMetadata.Description,
                Source = photoMetadata.Source,
                Id = photoMetadata.Id,
                PhotoDateUtc = photoMetadata.PhotoDateUtc,
                PhotoUrl = Url.Action("Photo", new {id = photoMetadata.Id}),
                ThumbnailUrl = Url.Action("Thumbnail", new {id = photoMetadata.Id}),
                Title = photoMetadata.Title,
                UserName = photoMetadata.UserName
            };
        }
    }
}