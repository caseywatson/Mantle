using System;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Identity.Commands
{
    public class UpdateUser : ICommand
    {
        public UpdateUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UpdateUser(MantleUser user)
            : this()
        {
            user.Require(nameof(user));

            User = user;
        }

        public MantleUser User { get; set; }

        public string Id { get; set; }
    }
}