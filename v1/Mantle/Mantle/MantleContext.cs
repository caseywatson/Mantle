using Mantle.Interfaces;

namespace Mantle
{
    public class MantleContext
    {
        public static MantleContext Current { get; set; }
        public IDependencyResolver DependencyResolver { get; set; }
    }
}