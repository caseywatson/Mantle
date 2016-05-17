using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserQueryService<T>
        where T : MantleUser
    {
        T FindUserByEmail(string email);
        T FindUserById(string id);
        T FindUserByName(string name);
        Task<T> FindUserByEmailAsync(string email);
        Task<T> FindUserByIdAsync(string id);
        Task<T> FindUserByNameAsync(string name);
    }
}
