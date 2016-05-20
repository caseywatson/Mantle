using System;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Identity.Commands
{
    public class DeleteUser : ICommand
    {
        public DeleteUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public DeleteUser(string userId)
            : this()
        {
            userId.Require(nameof(userId));

            UserId = userId;
        }

        public string UserId { get; set; }

        public string Id { get; set; }
    }
}