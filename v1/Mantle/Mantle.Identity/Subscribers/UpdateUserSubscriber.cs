using Mantle.Identity.Commands;
using Mantle.Identity.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Subscribers;

namespace Mantle.Identity.Subscribers
{
    public class UpdateUserSubscriber : BaseSubscriber<UpdateUser>
    {
        private readonly IMantleUserRepository<MantleUser> userRepository;

        public UpdateUserSubscriber(IMantleUserRepository<MantleUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public override void HandleMessage(IMessageContext<UpdateUser> messageContext)
        {
            userRepository.UpdateUser(messageContext.Message.User);
        }
    }
}