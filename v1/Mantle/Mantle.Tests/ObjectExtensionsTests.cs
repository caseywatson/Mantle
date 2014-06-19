using System;
using Mantle.Extensions;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void Should_convert_anonymous_object_to_dictionary()
        {
            var obj =
                new
                {
                    FirstName = "Casey",
                    LastName = "Watson",
                    Age = 33,
                    Birthdate = DateTime.Parse("1/11/1981"),
                    IsMarried = true
                };

            var objDictionary = obj.ToDictionary();

            Assert.IsNotNull(objDictionary);

            Assert.IsTrue(objDictionary.ContainsKey("FirstName"));
            Assert.IsTrue(objDictionary.ContainsKey("LastName"));
            Assert.IsTrue(objDictionary.ContainsKey("Age"));
            Assert.IsTrue(objDictionary.ContainsKey("Birthdate"));
            Assert.IsTrue(objDictionary.ContainsKey("IsMarried"));

            Assert.AreEqual(objDictionary["FirstName"], obj.FirstName);
            Assert.AreEqual(objDictionary["LastName"], obj.LastName);
            Assert.AreEqual(objDictionary["Age"], obj.Age);
            Assert.AreEqual(objDictionary["Birthdate"], obj.Birthdate);
            Assert.AreEqual(objDictionary["IsMarried"], obj.IsMarried);
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_attempting_to_convert_null_to_a_dictionary()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ObjectExtensions.ToDictionary(null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "source");
        }
    }
}