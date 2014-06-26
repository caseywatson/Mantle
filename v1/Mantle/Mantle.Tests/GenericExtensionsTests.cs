using System;
using Mantle.Extensions;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class GenericExtensionsTests
    {
        private const string TestObjectJson = "{\"Name\":\"Value\"}";

        public class TestObject
        {
            public string Name { get; set; }
        }

        [Test]
        public void Should_convert_Json_to_object()
        {
            var obj = TestObjectJson.FromJson<TestObject>();

            Assert.IsNotNull(obj);
            Assert.AreEqual(obj.Name, "Value");
        }

        [Test]
        public void Should_convert_object_to_Json()
        {
            var testObject = new TestObject {Name = "Value"};
            string jsonObject = testObject.ToJson();

            Assert.IsNotNull(jsonObject);
            Assert.AreEqual(TestObjectJson, jsonObject);
        }

        [Test]
        public void When_converting_Json_to_an_object_should_throw_exception_if_object_string_is_null()
        {
            var ex = Assert.Throws<ArgumentException>(() => (default(string)).FromJson<TestObject>());

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "objectString");
        }
    }
}