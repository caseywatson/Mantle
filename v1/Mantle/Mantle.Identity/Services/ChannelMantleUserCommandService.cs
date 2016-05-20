using System.Threading.Tasks;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;

namespace Mantle.Identity.Services
{
    public class ChannelMantleUserCommandService : IMantleUserCommandService<MantleUser>
    {
        private readonly IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory;

        private IPublisherChannel<MessageEnvelope> publisherChannel;

        public ChannelMantleUserCommandService(IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory)
        {
            this.publisherChannelDirectory = publisherChannelDirectory;
        }

        [Configurable(IsRequired = true)]
        public string UserCommandChannelName { get; set; }

        public void CreateUser(MantleUser user)
        {
            user.Require(nameof(user));

            GetUserCommandChannel().Publish(new CreateUser(user));
        }

        public void DeleteUser(string userId)
        {
            userId.Require(nameof(userId));

            GetUserCommandChannel().Publish(new DeleteUser(userId));
        }

        public void UpdateUser(MantleUser user)
        {
            user.Require(nameof(user));

            GetUserCommandChannel().Publish(new UpdateUser(user));
        }

        public Task CreateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            CreateUser(user);

            return Task.FromResult(0);
        }

        public Task DeleteUserAsync(string userId)
        {
            userId.Require(nameof(userId));

            DeleteUser(userId);

            return Task.FromResult(0);
        }

        public Task UpdateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            UpdateUser(user);

            return Task.FromResult(0);
        }

        private IPublisherChannel<MessageEnvelope> GetUserCommandChannel()
        {
            return (publisherChannel = (publisherChannel ??
                                        publisherChannelDirectory[UserCommandChannelName]));
        }
    }
}