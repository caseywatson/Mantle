using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantle.Identity.Interfaces;

namespace Mantle.Identity.Azure.Repositories
{
    public class DocumentDbMantleUserRepository<T> : IMantleUserRepository<T>
        where T : MantleUser
    {
        public void CreateUser(T user)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(T user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public T FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public T FindUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public T FindUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(T user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(T user)
        {
            throw new NotImplementedException();
        }
    }
}
