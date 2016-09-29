using System;
using Mantle.FaultTolerance.Strategies;
using NUnit.Framework;

namespace Mantle.Aws.Tests
{
    [TestFixture]
    public class AwsRegionEndpointsTests
    {
        [Test]
        public void Should_find_region_endpoint_given_valid_display_name()
        {
            const string providedDisplayName = "US East (Virginia)";
            const string expectedSystemName = "us-east-1";

            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            var regionEndpoint = regionEndpoints.GetRegionEndpointByName(providedDisplayName);

            Assert.IsNotNull(regionEndpoint);
            Assert.AreEqual(regionEndpoint.SystemName, expectedSystemName);
        }

        [Test]
        public void Should_find_region_endpoint_given_valid_system_name()
        {
            const string providedSystemName = "Us-East-1";
            const string expectedSystemName = "us-east-1";

            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            var regionEndpoint = regionEndpoints.GetRegionEndpointByName(providedSystemName);

            Assert.IsNotNull(regionEndpoint);
            Assert.AreEqual(regionEndpoint.SystemName, expectedSystemName);
        }

        [Test]
        public void Should_not_find_region_endpoint_given_invalid_name()
        {
            const string providedName = "Romulus";

            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            var regionEndpoint = regionEndpoints.GetRegionEndpointByName(providedName);

            Assert.IsNull(regionEndpoint);
        }

        [Test]
        public void Should_throw_exception_if_empty_name_is_provided()
        {
            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            Assert.Throws<ArgumentException>(() => regionEndpoints.GetRegionEndpointByName(string.Empty));
        }

        [Test]
        public void Should_throw_exception_if_null_name_is_provided()
        {
            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            Assert.Throws<ArgumentException>(() => regionEndpoints.GetRegionEndpointByName(null));
        }
    }
}