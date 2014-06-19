using Mantle.Configuration.Attributes;

namespace Mantle.Configuration.Tests.Targets
{
    public class RequiredTestTarget
    {
        [Configurable(IsRequired = true)]
        public string RequiredProperty { get; set; }
    }
}