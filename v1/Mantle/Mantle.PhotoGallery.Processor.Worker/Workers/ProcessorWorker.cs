using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantle.Hosting.Messaging.Workers;
using Mantle.Interfaces;

namespace Mantle.PhotoGallery.Processor.Worker.Workers
{
    public class ProcessorWorker : SubscriptionWorker
    {
        public ProcessorWorker(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
        }
    }
}
