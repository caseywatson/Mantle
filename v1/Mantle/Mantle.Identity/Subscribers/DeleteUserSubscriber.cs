using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Subscribers;

namespace Mantle.Identity.Subscribers
{
    public class DeleteUserSubscriber : BaseSubscriber<DeleteUser>
    {
        private readonly IMantleUserRepository<MantleUser> userRepository;

        public DeleteUserSubscriber(IMantleUserRepository<MantleUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public override void HandleMessage(IMessageContext<DeleteUser> messageContext)
        {
            userRepository.DeleteUser(messageContext.Message.UserId);
        }
    }
}