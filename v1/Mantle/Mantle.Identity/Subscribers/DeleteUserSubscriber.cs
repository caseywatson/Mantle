using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Identity.Subscribers
{
    public class DeleteUserSubscriber : ISubscriber<DeleteUser>
    {
        private readonly IMantleUserRepository<MantleUser> userRepository;

        public DeleteUserSubscriber(IMantleUserRepository<MantleUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void HandleMessage(IMessageContext<DeleteUser> messageContext)
        {
            userRepository.DeleteUser(messageContext.Message.UserId);
        }
    }
}