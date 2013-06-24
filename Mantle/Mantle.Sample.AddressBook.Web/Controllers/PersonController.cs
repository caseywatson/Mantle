using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mantle.Sample.AddressBook.Shared;

namespace Mantle.Sample.AddressBook.Web.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Person person)
        {
            if (ModelState.IsValid == false)
                return View(person);

            return View();
        }
    }
}
