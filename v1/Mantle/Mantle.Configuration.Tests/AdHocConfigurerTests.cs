using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Mantle.Configuration.Attributes;
using Mantle.Configuration.Configurers;
using Mantle.Configuration.Extensions;
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
        public void Should_throw_ArgumentNullException_if_instantiated_with_null_configuration_object()
        {
            Assert.Throws<ArgumentNullException>(() => new AdHocConfigurer<string>(null));
        }

        [Test]
        public void Should_serialize_configuration_object_properties_to_configuration_settings()
        {
            var testObj = new {Name = "Value"};
            var testTarget = new TestObject();
            var testConfigurer = new AdHocConfigurer<TestObject>(testObj);

            var configurationSettings =
                testConfigurer.GetConfigurationSettings(testTarget.ToConfigurableObject()).ToList();

            Console.WriteLine(configurationSettings.ToList());
        }
    }
}
