using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Mantle.Sample.AddressBook.Shared
{
    [DataContract(Namespace = "http://mantle/samples/addressbook/1")]
    public class Person
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }

        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string ZipCode { get; set; }
    }
}