using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mantle.Extensions;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Mantle.Identity.Services
{
    public class CqrsMantleUserService : IMantleUserService<MantleUser>
    {
        private readonly IList<IMantleUserCommandService<MantleUser>> userCommandServices;
        private readonly IMantleUserQueryService<MantleUser> userQueryService;

        public CqrsMantleUserService(IMantleUserCommandService<MantleUser>[] userCommandServices,
                                     IMantleUserQueryService<MantleUser> userQueryService)
        {
            this.userCommandServices = userCommandServices.ToList();
            this.userQueryService = userQueryService;
        }

        public void CreateUser(MantleUser user)
        {
            user.Require(nameof(user));

            foreach (var userCommandService in userCommandServices)
                userCommandService.CreateUser(user);
        }

        public Task CreateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            return Task.WhenAll(userCommandServices.Select(s => s.CreateUserAsync(user)).ToList());
        }

        public void DeleteUser(string userId)
        {
            userId.Require(nameof(userId));

            foreach (var userCommandService in userCommandServices)
                userCommandService.DeleteUser(userId);
        }

        public Task DeleteUserAsync(string userId)
        {
            userId.Require(nameof(userId));

            return Task.WhenAll(userCommandServices.Select(s => s.DeleteUserAsync(userId)).ToList());
        }

        public MantleUser FindUserByEmail(string email)
        {
            email.Require(nameof(email));

            return userQueryService.FindUserByEmail(email);
        }

        public Task<MantleUser> FindUserByEmailAsync(string email)
        {
            email.Require(nameof(email));

            return userQueryService.FindUserByEmailAsync(email);
        }

        public MantleUser FindUserById(string id)
        {
            id.Require(nameof(id));

            return userQueryService.FindUserById(id);
        }

        public Task<MantleUser> FindUserByIdAsync(string id)
        {
            id.Require(nameof(id));

            return userQueryService.FindUserByIdAsync(id);
        }

        public MantleUser FindUserByLogin(UserLoginInfo loginInfo)
        {
            loginInfo.Require(nameof(loginInfo));

            return userQueryService.FindUserByLogin(loginInfo);
        }

        public Task<MantleUser> FindUserByLoginAsync(UserLoginInfo loginInfo)
        {
            loginInfo.Require(nameof(loginInfo));

            return userQueryService.FindUserByLoginAsync(loginInfo);
        }

        public MantleUser FindUserByName(string name)
        {
            name.Require(nameof(name));

            return userQueryService.FindUserByName(name);
        }

        public Task<MantleUser> FindUserByNameAsync(string name)
        {
            name.Require(nameof(name));

            return userQueryService.FindUserByNameAsync(name);
        }

        public void UpdateUser(MantleUser user)
        {
            user.Require(nameof(user));

            foreach (var userCommandService in userCommandServices)
                userCommandService.UpdateUser(user);
        }

        public Task UpdateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            return Task.WhenAll(userCommandServices.Select(s => s.UpdateUserAsync(user)).ToList());
        }
    }
}