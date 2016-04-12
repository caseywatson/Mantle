using Amazon;
using Mantle.Aws.Interfaces;
using Mantle.Extensions;
using System.Linq;

namespace Mantle.Aws
{
    public class AwsRegionEndpoints : IAwsRegionEndpoints
    {
        public RegionEndpoint GetRegionEndpointByName(string regionName)
        {
            regionName.Require(nameof(regionName));

            regionName = regionName.ToLower();

            return RegionEndpoint.EnumerableAllRegions.SingleOrDefault(
                re => (re.DisplayName.ToLower() == regionName) || 
                      (re.SystemName.ToLower() == regionName));
        }
    }
}
