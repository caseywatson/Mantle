using System.Linq;
using Amazon;
using Mantle.Aws.Interfaces;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;

namespace Mantle.Aws
{
    public class AwsRegionEndpoints : IAwsRegionEndpoints
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AwsRegionEndpoints(ITransientFaultStrategy transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        public RegionEndpoint GetRegionEndpointByName(string regionName)
        {
            regionName.Require(nameof(regionName));

            regionName = regionName.ToLower();

            var allRegions = transientFaultStrategy.Try(() => RegionEndpoint.EnumerableAllRegions.ToList());

            return allRegions.SingleOrDefault(re => (re.DisplayName.ToLower() == regionName) ||
                                                    (re.SystemName.ToLower() == regionName));
        }
    }
}