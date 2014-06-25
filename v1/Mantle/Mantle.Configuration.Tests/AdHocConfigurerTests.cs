using System;
using Mantle.Configuration.Attributes;
using Mantle.Configuration.Configurers;
using NUnit.Framework;

namespace Mantle.Configuration.Tests
{
    [TestFixture]
    public class AdHocConfigurerTests
    {
        public class TestObject
        {
            [Configurable]
            public string Name { get; set; }
        }

        [Test]
        public void Should_configure_property_if_setting_is_supplied()
        {
            var obj = new TestObject();
            var cfgSettingsObj = new {Name = "Value"};
            var configurer = new AdHocConfigurer<TestObject>(cfgSettingsObj);

            configurer.Configure(obj);

            Assert.IsNotNull(obj.Name);
            Assert.AreEqual(obj.Name, cfgSettingsObj.Name);
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_instantiated_with_null_configuration_object()
        {
            Assert.Throws<ArgumentNullException>(() => new AdHocConfigurer<TestObject>(null));
        }
    }
}