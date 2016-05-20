using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Identity.Subscribers
{
    public class UpdateUserSubscriber : ISubscriber<UpdateUser>
    {
        private readonly IMantleUserRepository<MantleUser> userRepository;

        public UpdateUserSubscriber(IMantleUserRepository<MantleUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void HandleMessage(IMessageContext<UpdateUser> messageContext)
        {
            userRepository.UpdateUser(messageContext.Message.User);
        }
    }
}