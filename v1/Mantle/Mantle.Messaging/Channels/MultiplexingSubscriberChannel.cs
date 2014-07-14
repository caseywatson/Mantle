using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Channels
{
    public class MultiplexingSubscriberChannel<T> : ISubscriberChannel<T>
        where T : class
    {
        private readonly ISubscriberChannel<T>[] childChannels;

        public MultiplexingSubscriberChannel(params ISubscriberChannel<T>[] childChannels)
        {
            childChannels.Require("childChannels");
            this.childChannels = childChannels;
        }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var receiveAsyncTasksQuery =
                childChannels.Select(cc => ReceiveAsync(cc, cancellationTokenSource.Token, timeout));

            var receiveAsyncTasks = receiveAsyncTasksQuery.ToArray();
            var firstFinishedTask = Task.WhenAny(receiveAsyncTasks).Result;

            cancellationTokenSource.Cancel();

            return firstFinishedTask.Result;
        }

        private async Task<IMessageContext<T>> ReceiveAsync(ISubscriberChannel<T> channel,
                                                            CancellationToken cancellationToken, TimeSpan? timeout)
        {
            return await Task.Run(() => channel.Receive(timeout), cancellationToken);
        }
    }
}