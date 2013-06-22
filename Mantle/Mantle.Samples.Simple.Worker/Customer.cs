using System.Runtime.Serialization;

namespace Mantle.Samples.Simple.Worker
{
    [DataContract(Namespace = "http://mantle/samples/1")]
    public class Customer
    {
        [DataMember(Order = 0)]
        public string Id { get; set; }

        [DataMember(Order = 1)]
        public string FirstName { get; set; }

        [DataMember(Order = 2)]
        public string LastName { get; set; }

        [DataMember(Order = 3)]
        public string EmailAddress { get; set; }

        [DataMember(Order = 4)]
        public string PhoneNumber { get; set; }
    }
}