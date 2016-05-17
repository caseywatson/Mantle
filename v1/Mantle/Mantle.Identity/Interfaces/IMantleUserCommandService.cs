using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserCommandService<T>
        where T : MantleUser
    {
        void CreateUser(T user);
        void DeleteUser(string userId);
        void UpdateUser(T user);
        Task CreateUserAsync(T user);
        Task DeleteUserAsync(string userId);
        Task UpdateUserAsync(T user);
    }
}
