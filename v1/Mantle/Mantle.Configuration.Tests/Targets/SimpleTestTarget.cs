using System;
using Mantle.Configuration.Attributes;

namespace Mantle.Configuration.Tests.Targets
{
    public class SimpleTestTarget
    {
        [Configurable]
        public bool BooleanProperty { get; set; }

        [Configurable]
        public double DoubleProperty { get; set; }

        [Configurable]
        public DateTime DateTimeProperty { get; set; }

        [Configurable]
        public Guid GuidProperty { get; set; }

        [Configurable]
        public string StringProperty { get; set; }

        [Configurable]
        public int IntProperty { get; set; }

        [Configurable]
        public long LongProperty { get; set; }

        [Configurable]
        public bool? NullableBooleanProperty { get; set; }

        [Configurable]
        public double? NullableDoubleProperty { get; set; }

        [Configurable]
        public DateTime? NullableDateTimeProperty { get; set; }

        [Configurable]
        public Guid? NullableGuidProperty { get; set; }

        [Configurable]
        public int? NullableIntProperty { get; set; }

        [Configurable]
        public long? NullableLongProperty { get; set; }

        [Configurable(SettingName = "CustomSettingNameProperty")]
        public string CustomSettingNameProperty { get; set; }

        [Configurable(SettingName = "{Name}.CustomSettingNameProperty")]
        public string NamedTargetCustomSettingNameProperty { get; set; }
    }
}