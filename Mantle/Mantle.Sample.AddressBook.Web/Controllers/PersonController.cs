using System.Web.Mvc;
using Mantle.Messaging;
using Mantle.Sample.AddressBook.Shared;

namespace Mantle.Sample.AddressBook.Web.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPublisherEndpointDirectory publisherEndpoints;

        public PersonController(IPublisherEndpointDirectory publisherEndpoints)
        {
            this.publisherEndpoints = publisherEndpoints;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Person model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            IPublisherClient publisherClient = publisherEndpoints["PersonQueue"].GetClient();

            publisherClient.Publish(model);

            return null;
        }
    }
}