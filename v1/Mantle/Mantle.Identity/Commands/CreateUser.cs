using System;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Identity.Commands
{
    public class CreateUser : ICommand
    {
        public CreateUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public CreateUser(MantleUser user)
            : this()
        {
            user.Require(nameof(user));

            User = user;
        }

        public MantleUser User { get; set; }

        public string Id { get; set; }
    }
}