using System.Collections.Generic;
using System.Linq;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle
{
    public class MantleContext
    {
        public MantleContext()
        {
        }

        public MantleContext(IDependencyResolver dependencyResolver,
                             IEnumerable<string> loadedProfiles)
        {
            dependencyResolver.Require(nameof(dependencyResolver));
            loadedProfiles.Require(nameof(loadedProfiles));

            DependencyResolver = dependencyResolver;
            LoadedProfiles = loadedProfiles.ToList();
        }

        public static MantleContext Current { get; set; }

        public IDependencyResolver DependencyResolver { get; set; }
        public IEnumerable<string> LoadedProfiles { get; set; }
    }
}