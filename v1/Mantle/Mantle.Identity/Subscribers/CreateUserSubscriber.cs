using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Identity.Subscribers
{
    public class CreateUserSubscriber : ISubscriber<CreateUser>
    {
        private readonly IMantleUserRepository<MantleUser> userRepository;

        public CreateUserSubscriber(IMantleUserRepository<MantleUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void HandleMessage(IMessageContext<CreateUser> messageContext)
        {
            userRepository.CreateUser(messageContext.Message.User);
        }
    }
}