using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserQueryService<TUser>
        where TUser : MantleUser
    {
        TUser FindUserByEmail(string email);
        TUser FindUserById(string id);
        TUser FindUserByLogin(UserLoginInfo loginInfo);
        TUser FindUserByName(string name);
        Task<TUser> FindUserByEmailAsync(string email);
        Task<TUser> FindUserByIdAsync(string id);
        Task<TUser> FindUserByLoginAsync(UserLoginInfo loginInfo);
        Task<TUser> FindUserByNameAsync(string name);
    }
}