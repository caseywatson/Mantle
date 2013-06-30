using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Mantle.Sample.AddressBook.Shared
{
    [DataContract(Namespace = "http://mantle/samples/addressbook/1")]
    public class Person
    {
        public Person()
        {
            Id = Guid.NewGuid().ToString();
        }

        [DataMember(Order = 0)]
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [DataMember(Order = 1)]
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [DataMember(Order = 2)]
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [DataMember(Order = 3)]
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }

        [DataMember(Order = 4)]
        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public string PhoneNumber { get; set; }

        [DataMember(Order = 5)]
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [DataMember(Order = 6)]
        [DisplayName("City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [DataMember(Order = 7)]
        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [DataMember(Order = 8)]
        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string ZipCode { get; set; }
    }
}