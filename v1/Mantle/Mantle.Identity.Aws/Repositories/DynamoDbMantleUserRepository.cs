using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Mantle.Identity.Aws.Repositories
{
    public class DynamoDbMantleUserRepository : IMantleUserRepository<MantleUser>, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void CreateUser(MantleUser user)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(MantleUser user)
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

        public MantleUser FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<MantleUser> FindUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public MantleUser FindUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<MantleUser> FindUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public MantleUser FindUserByLogin(UserLoginInfo loginInfo)
        {
            throw new NotImplementedException();
        }

        public Task<MantleUser> FindUserByLoginAsync(UserLoginInfo loginInfo)
        {
            throw new NotImplementedException();
        }

        public MantleUser FindUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<MantleUser> FindUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(MantleUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(MantleUser user)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, AttributeValue> ToDocumentDictionary(MantleUser user)
        {
            var docDictionary = new Dictionary<string, AttributeValue>();

            docDictionary[nameof(user.Id)] = new AttributeValue {S = user.Id};

            docDictionary[nameof(user.EmailConfirmed)] = new AttributeValue {BOOL = user.EmailConfirmed};
            docDictionary[nameof(user.LockoutEnabled)] = new AttributeValue {BOOL = user.LockoutEnabled};
            docDictionary[nameof(user.PhoneNumberConfirmed)] = new AttributeValue {BOOL = user.PhoneNumberConfirmed};
            docDictionary[nameof(user.TwoFactorEnabled)] = new AttributeValue {BOOL = user.TwoFactorEnabled};

            docDictionary[nameof(user.LockoutEndDate)] = new AttributeValue {S = user.LockoutEndDate.ToString("o")};

            docDictionary[nameof(user.AccessFailedCount)] = new AttributeValue {N = user.AccessFailedCount.ToString()};

            docDictionary[nameof(user.Claims)] = new AttributeValue
            {
                L = user.Claims.Select(c => new AttributeValue {M = ToDocumentDictionary(c)}).ToList()
            };

            docDictionary[nameof(user.Logins)] = new AttributeValue
            {
                L = user.Logins.Select(l => new AttributeValue {M = ToDocumentDictionary(l)}).ToList()
            };

            docDictionary[nameof(user.Roles)] = new AttributeValue {SS = user.Roles};

            docDictionary[nameof(user.Email)] = new AttributeValue {S = user.Email};
            docDictionary[nameof(user.PasswordHash)] = new AttributeValue {S = user.PasswordHash};
            docDictionary[nameof(user.PhoneNumber)] = new AttributeValue {S = user.PhoneNumber};
            docDictionary[nameof(user.SecurityStamp)] = new AttributeValue {S = user.SecurityStamp};
            docDictionary[nameof(user.UserName)] = new AttributeValue {S = user.UserName};

            return docDictionary;
        }

        private MantleUser ToMantleUser(Dictionary<string, AttributeValue> docDictionary)
        {
            var user = new MantleUser();

            user.Id = docDictionary[nameof(user.Id)].S;

            user.EmailConfirmed = docDictionary[nameof(user.EmailConfirmed)].BOOL;
            user.LockoutEnabled = docDictionary[nameof(user.LockoutEnabled)].BOOL;
            user.PhoneNumberConfirmed = docDictionary[nameof(user.PhoneNumberConfirmed)].BOOL;
            user.TwoFactorEnabled = docDictionary[nameof(user.TwoFactorEnabled)].BOOL;

            user.LockoutEndDate = DateTimeOffset.Parse(docDictionary[nameof(user.LockoutEnabled)].S);

            user.AccessFailedCount = int.Parse(docDictionary[nameof(user.AccessFailedCount)].N);

            user.Claims = docDictionary[nameof(user.Claims)].L.Select(av => ToMantleUserClaim(av.M)).ToList();

            // TODO: Going to grab Emily a Starbucks. Pick back up here...

            return user;
        }

        private Dictionary<string, AttributeValue> ToDocumentDictionary(MantleUserClaim claim)
        {
            var docDictionary = new Dictionary<string, AttributeValue>();

            docDictionary[nameof(claim.Id)] = new AttributeValue {S = claim.Id};
            docDictionary[nameof(claim.UserId)] = new AttributeValue {S = claim.UserId};
            docDictionary[nameof(claim.ClaimType)] = new AttributeValue {S = claim.ClaimType};
            docDictionary[nameof(claim.ClaimValue)] = new AttributeValue {S = claim.ClaimValue};

            return docDictionary;
        }

        private MantleUserClaim ToMantleUserClaim(Dictionary<string, AttributeValue> docDictionary)
        {
            var claim = new MantleUserClaim();

            claim.Id = docDictionary[nameof(claim.Id)].S;
            claim.UserId = docDictionary[nameof(claim.UserId)].S;
            claim.ClaimType = docDictionary[nameof(claim.ClaimType)].S;
            claim.ClaimValue = docDictionary[nameof(claim.ClaimValue)].S;

            return claim;
        }

        private Dictionary<string, AttributeValue> ToDocumentDictionary(MantleUserLogin login)
        {
            var docDictionary = new Dictionary<string, AttributeValue>();

            docDictionary[nameof(login.Id)] = new AttributeValue {S = login.Id};
            docDictionary[nameof(login.UserId)] = new AttributeValue {S = login.UserId};
            docDictionary[nameof(login.LoginProvider)] = new AttributeValue {S = login.LoginProvider};
            docDictionary[nameof(login.ProviderKey)] = new AttributeValue {S = login.ProviderKey};

            return docDictionary;
        }
    }
}