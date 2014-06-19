using System;
using System.Collections.Generic;
using System.Configuration;
using Mantle.Configuration.Tests.Configurers;
using Mantle.Configuration.Tests.Targets;
using NUnit.Framework;

namespace Mantle.Configuration.Tests
{
    [TestFixture]
    public class BaseConfigurerTests
    {
        [Test]
        public void Should_configure_Date_Time_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.DateTimeProperty", "1/11/1981")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.DateTimeProperty, DateTime.Parse("1/11/1981"));
        }

        [Test]
        public void Should_configure_Guid_property()
        {
            var testGuid = Guid.NewGuid();
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.GuidProperty", testGuid.ToString())
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.GuidProperty, testGuid);
        }

        [Test]
        public void Should_configure_boolean_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.BooleanProperty", "true")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsTrue(testTarget.BooleanProperty);
        }

        [Test]
        public void Should_configure_double_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.DoubleProperty", "1.0")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.DoubleProperty, 1.0);
        }

        [Test]
        public void Should_configure_int_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.IntProperty", "1")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.IntProperty, 1);
        }

        [Test]
        public void Should_configure_long_property()
        {
            var configurer = new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
            {
                new ConfigurationSetting("SimpleTestTarget.LongProperty", "1")
            });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.LongProperty, 1);
        }

        [Test]
        public void Should_configure_nullable_Date_Time_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableDateTimeProperty", "1/11/1981")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableDateTimeProperty);
            Assert.AreEqual(testTarget.NullableDateTimeProperty, DateTime.Parse("1/11/1981"));
        }

        [Test]
        public void Should_configure_nullable_Guid_property()
        {
            var testGuid = Guid.NewGuid();
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableGuidProperty", testGuid.ToString())
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableGuidProperty);
            Assert.AreEqual(testTarget.NullableGuidProperty, testGuid);
        }

        [Test]
        public void Should_configure_nullable_boolean_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableBooleanProperty", "true")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableBooleanProperty);
            Assert.IsTrue(testTarget.NullableBooleanProperty.Value);
        }

        [Test]
        public void Should_configure_nullable_double_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableDoubleProperty", "1.0")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableDoubleProperty);
            Assert.AreEqual(testTarget.NullableDoubleProperty, 1.0);
        }

        [Test]
        public void Should_configure_nullable_int_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableIntProperty", "1")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableIntProperty);
            Assert.AreEqual(testTarget.NullableIntProperty, 1);
        }

        [Test]
        public void Should_configure_nullable_long_property()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableLongProperty", "1")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.NullableLongProperty);
            Assert.AreEqual(testTarget.NullableLongProperty, 1);
        }

        [Test]
        public void Should_configure_property_with_custom_setting_name()
        {
            const string testString = "Test";
            const string settingName = "CustomSettingNameProperty";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, testString)
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNotNull(testTarget.CustomSettingNameProperty);
            Assert.AreEqual(testTarget.CustomSettingNameProperty, testString);
        }

        [Test]
        public void Should_configure_property_with_custom_setting_name_on_named_target()
        {
            const string targetName = "Test";
            const string testString = "Test";
            const string settingName = "Test.CustomSettingNameProperty";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, testString)
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget, targetName);

            Assert.IsNotNull(testTarget.NamedTargetCustomSettingNameProperty);
            Assert.AreEqual(testTarget.NamedTargetCustomSettingNameProperty, testString);
        }

        [Test]
        public void Should_configure_string_property()
        {
            const string testString = "Test";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.StringProperty", testString)
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.AreEqual(testTarget.StringProperty, testString);
        }

        [Test]
        public void Should_follow_convention_by_default_when_configuring_properties_on_named_target()
        {
            const string targetName = "TestTarget";
            const string testString = "Test";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(String.Format("{0}.SimpleTestTarget.StringProperty", targetName),
                                             testString)
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget, targetName);

            Assert.AreEqual(testTarget.StringProperty, testString);
        }

        [Test]
        public void Should_set_nullable_Date_Time_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableDateTimeProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableDateTimeProperty);
        }

        [Test]
        public void Should_set_nullable_Guid_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableGuidProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableDateTimeProperty);
        }

        [Test]
        public void Should_set_nullable_boolean_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableBooleanProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableDateTimeProperty);
        }

        [Test]
        public void Should_set_nullable_double_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableDoubleProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableDateTimeProperty);
        }

        [Test]
        public void Should_set_nullable_int_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableIntProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableDateTimeProperty);
        }

        [Test]
        public void Should_set_nullable_long_property_to_null_if_configuration_value_can_not_be_parsed()
        {
            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting("SimpleTestTarget.NullableLongProperty", "Invalid")
                });

            var testTarget = new SimpleTestTarget();

            configurer.Configure(testTarget);

            Assert.IsNull(testTarget.NullableLongProperty);
        }

        [Test]
        public void
            Should_throw_ConfigurationErrorsException_if_Date_Time_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "DateTimeProperty";
            const string settingName = "SimpleTestTarget.DateTimeProperty";
            const string settingValue = "Invalid 1/11/1981";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a date/time value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_Guid_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "GuidProperty";
            const string settingName = "SimpleTestTarget.GuidProperty";
            const string settingValue = "Invalid";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a GUID value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_boolean_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "BooleanProperty";
            const string settingName = "SimpleTestTarget.BooleanProperty";
            const string settingValue = "Invalid";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a boolean value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_double_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "DoubleProperty";
            const string settingName = "SimpleTestTarget.DoubleProperty";
            const string settingValue = "Invalid";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a double value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_int_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "IntProperty";
            const string settingName = "SimpleTestTarget.IntProperty";
            const string settingValue = "Invalid";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a 32-bit integer value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_long_property_configuration_value_can_not_be_parsed()
        {
            const string propertyName = "LongProperty";
            const string settingName = "SimpleTestTarget.LongProperty";
            const string settingValue = "Invalid";

            var configurer =
                new TestConfigurer<SimpleTestTarget>(new List<ConfigurationSetting>
                {
                    new ConfigurationSetting(settingName, settingValue)
                });

            var testTarget = new SimpleTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith(String.Format(
                                                              "Unable to apply configuration setting [{0}: {1}] to property [{2}/{3}]. [{1}] can not be converted to a 64-bit integer value.",
                                                              settingName, settingValue, typeof (SimpleTestTarget).Name,
                                                              propertyName)));
        }

        [Test]
        public void Should_throw_ConfigurationErrorsException_if_required_property_is_not_configured()
        {
            var configurer = new TestConfigurer<RequiredTestTarget>(new List<ConfigurationSetting>());
            var testTarget = new RequiredTestTarget();

            var ex = Assert.Throws<ConfigurationErrorsException>(() => configurer.Configure(testTarget));

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.StartsWith("[RequiredTestTarget.RequiredProperty] is not configured."));
        }
    }
}