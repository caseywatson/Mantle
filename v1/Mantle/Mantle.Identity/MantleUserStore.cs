using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mantle.Extensions;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Mantle.Identity
{
    public class MantleUserStore<T> :
        IUserLoginStore<T>,
        IUserClaimStore<T>,
        IUserRoleStore<T>,
        IUserPasswordStore<T>,
        IUserSecurityStampStore<T>,
        IUserStore<T>,
        IUserEmailStore<T>,
        IUserLockoutStore<T, string>,
        IUserTwoFactorStore<T, string>,
        IUserPhoneNumberStore<T>
        where T : MantleUser
    {
        private readonly IMantleUserService<T> userService;

        private bool isDisposed;

        public MantleUserStore(IMantleUserService<T> userService)
        {
            this.userService = userService;
        }

        public Task AddClaimAsync(T user, Claim claim)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));
            claim.Require(nameof(claim));

            if (user.Claims.None(c => (c.ClaimType == claim.Type) && (c.ClaimValue == claim.Value)))
                user.Claims.Add(new MantleUserClaim(user.Id, claim.Type, claim.Value));

            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.Claims
                                       .Select(c => new Claim(c.ClaimType, c.ClaimValue))
                                       .ToList() as IList<Claim>);
        }

        public Task RemoveClaimAsync(T user, Claim claim)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));
            claim.Require(nameof(claim));

            user.Claims.RemoveAll(c => (c.ClaimType == claim.Type) && (c.ClaimValue == claim.Value));

            return Task.FromResult(0);
        }

        public Task SetEmailAsync(T user, string email)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));
            email.Require(nameof(email));

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task<T> FindByEmailAsync(string email)
        {
            ThrowExceptionIfDisposed();

            email.Require(nameof(email));

            return userService.FindUserByEmailAsync(email);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(T user, DateTimeOffset lockoutEnd)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            user.LockoutEndDate = lockoutEnd;

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            user.AccessFailedCount++;

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(T user, bool enabled)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task AddLoginAsync(T user, UserLoginInfo login)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));
            login.Require(nameof(login));

            if (user.Logins.None(l => (l.LoginProvider == login.LoginProvider) && (l.ProviderKey == login.ProviderKey)))
                user.Logins.Add(new MantleUserLogin(user.Id, login.LoginProvider, login.ProviderKey));

            return userService.UpdateUserAsync(user);
        }

        public Task CreateAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return userService.CreateUserAsync(user);
        }

        public Task DeleteAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return userService.DeleteUserAsync(user.Id);
        }

        public void Dispose()
        {
            isDisposed = true;
        }

        public Task<T> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            ThrowExceptionIfDisposed();

            userId.Require(nameof(userId));

            return userService.FindUserByIdAsync(userId);
        }

        public Task<T> FindByNameAsync(string userName)
        {
            ThrowExceptionIfDisposed();

            userName.Require(nameof(userName));

            return userService.FindUserByNameAsync(userName);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return Task.FromResult(user.Logins
                                       .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey))
                                       .ToList() as IList<UserLoginInfo>);
        }

        public Task RemoveLoginAsync(T user, UserLoginInfo login)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));
            login.Require(nameof(login));

            user.Logins.RemoveAll(l => (l.LoginProvider == login.LoginProvider) && (l.ProviderKey == login.ProviderKey));

            return Task.FromResult(0);
        }

        public Task UpdateAsync(T user)
        {
            ThrowExceptionIfDisposed();

            user.Require(nameof(user));

            return userService.UpdateUserAsync(user);
        }

        public Task SetPasswordHashAsync(T user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberAsync(T user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(T user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(T user, string stamp)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(T user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(T user)
        {
            throw new NotImplementedException();
        }

        private void ThrowExceptionIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(MantleUserStore<T>));
        }
    }
}