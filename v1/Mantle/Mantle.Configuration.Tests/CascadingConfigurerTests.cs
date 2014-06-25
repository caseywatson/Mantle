using System;
using Mantle.Configuration.Attributes;
using Mantle.Configuration.Configurers;
using NUnit.Framework;

namespace Mantle.Configuration.Tests
{
    [TestFixture]
    public class CascadingConfigurerTests
    {
        public class TestObject
        {
            [Configurable]
            public string Property1 { get; set; }

            [Configurable]
            public string Property2 { get; set; }

            [Configurable]
            public string Property3 { get; set; }
        }

        [Test]
        public void Should_cascade_configuration_settings()
        {
            var configurerA = new AdHocConfigurer<TestObject>(new {Property1 = "A", Property2 = "A", Property3 = "A"});
            var configurerB = new AdHocConfigurer<TestObject>(new {Property1 = "B", Property2 = "B"});
            var configurerC = new AdHocConfigurer<TestObject>(new {Property1 = "C"});

            var configurer = new CascadingConfigurer<TestObject>(configurerA, configurerB, configurerC);
            var testObject = new TestObject();

            configurer.Configure(testObject);

            Assert.IsNotNull(testObject.Property1);
            Assert.IsNotNull(testObject.Property2);
            Assert.IsNotNull(testObject.Property3);

            Assert.AreEqual(testObject.Property1, "C");
            Assert.AreEqual(testObject.Property2, "B");
            Assert.AreEqual(testObject.Property3, "A");
        }

        [Test]
        public void Should_throw_ArgumentException_if_no_configurers_are_supplied()
        {
            Assert.Throws<ArgumentException>(() => new CascadingConfigurer<TestObject>());
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_null_collection_of_configurers_is_supplied()
        {
            Assert.Throws<ArgumentNullException>(() => new CascadingConfigurer<TestObject>(null));
        }
    }
}