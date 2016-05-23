using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle
{
    public class MantleContext
    {
        public MantleContext()
        {
        }

        public MantleContext(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Require(nameof(dependencyResolver));

            DependencyResolver = dependencyResolver;
        }

        public static MantleContext Current { get; set; }
        public IDependencyResolver DependencyResolver { get; set; }
    }
}