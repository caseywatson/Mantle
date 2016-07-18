using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Identity.Azure.Entities;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Mantle.Identity.Azure.Repositories
{
    public class DocumentDbMantleUserRepository : IDisposable, IMantleUserRepository<MantleUser>
    {
        private static readonly MapperConfiguration mapperConfiguration;

        private readonly ITransientFaultStrategy transientFaultStrategy;

        private DocumentClient documentClient;
        private bool doesCollectionExist;

        private bool doesDbExist;

        static DocumentDbMantleUserRepository()
        {
            mapperConfiguration = new MapperConfiguration(mc =>
            {
                mc.CreateMap<MantleUser, DocumentDbMantleUser>().ReverseMap();
                mc.CreateMap<MantleUserClaim, DocumentDbMantleUserClaim>().ReverseMap();
                mc.CreateMap<MantleUserLogin, DocumentDbMantleUserLogin>().ReverseMap();
            });
        }

        public DocumentDbMantleUserRepository(ITransientFaultStrategy transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;

            AutoSetup = true;
            DocumentDbDatabaseId = "MantleUsers";
            DocumentDbCollectionId = "MantleUsers";
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string DocumentDbEndpointUrl { get; set; }

        [Configurable(IsRequired = true)]
        public string DocumentDbAuthKey { get; set; }

        [Configurable]
        public string DocumentDbDatabaseId { get; set; }

        [Configurable]
        public string DocumentDbCollectionId { get; set; }

        public DocumentClient DocumentClient => GetDocumentClient();

        public void Dispose()
        {
            documentClient?.Dispose();
        }

        public void CreateUser(MantleUser user)
        {
            user.Require(nameof(user));

            CreateUserAsync(user).Wait();
        }

        public async Task CreateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = mapperConfiguration.CreateMapper().Map<DocumentDbMantleUser>(user);

            await transientFaultStrategy.Try(
                () => DocumentClient.UpsertDocumentAsync(documentCollectionUri, documentDbUser));
        }

        public void DeleteUser(string userId)
        {
            userId.Require(nameof(userId));

            DeleteUserAsync(userId).Wait();
        }

        public async Task DeleteUserAsync(string userId)
        {
            userId.Require(nameof(userId));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = transientFaultStrategy.Try(
                () => DocumentClient.CreateDocumentQuery<DocumentDbMantleUser>(documentCollectionUri)
                    .Where(d => (d.Id == userId))
                    .AsEnumerable()
                    .FirstOrDefault());

            if (documentDbUser == null)
                throw new InvalidOperationException($"User [{userId}] not found.");

            var documentUri =
                UriFactory.CreateDocumentUri(DocumentDbDatabaseId, DocumentDbCollectionId, userId);

            await transientFaultStrategy.Try(() => DocumentClient.DeleteDocumentAsync(documentUri));
        }

        public MantleUser FindUserByEmail(string email)
        {
            email.Require(nameof(email));

            return FindUserByEmailAsync(email).Result;
        }

        public async Task<MantleUser> FindUserByEmailAsync(string email)
        {
            email.Require(nameof(email));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = transientFaultStrategy.Try(
                () => DocumentClient.CreateDocumentQuery<DocumentDbMantleUser>(documentCollectionUri)
                    .Where(d => (d.Email == email))
                    .AsEnumerable()
                    .FirstOrDefault());

            if (documentDbUser == null)
                return null;

            return mapperConfiguration.CreateMapper().Map<MantleUser>(documentDbUser);
        }

        public MantleUser FindUserById(string id)
        {
            id.Require(nameof(id));

            return FindUserByIdAsync(id).Result;
        }

        public async Task<MantleUser> FindUserByIdAsync(string id)
        {
            id.Require(nameof(id));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = transientFaultStrategy.Try(
                () => DocumentClient.CreateDocumentQuery<DocumentDbMantleUser>(documentCollectionUri)
                    .Where(d => (d.Id == id))
                    .AsEnumerable()
                    .FirstOrDefault());

            if (documentDbUser == null)
                return null;

            return mapperConfiguration.CreateMapper().Map<MantleUser>(documentDbUser);
        }

        public MantleUser FindUserByName(string name)
        {
            name.Require(nameof(name));

            return FindUserByNameAsync(name).Result;
        }

        public async Task<MantleUser> FindUserByNameAsync(string name)
        {
            name.Require(nameof(name));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = transientFaultStrategy.Try(
                () => DocumentClient.CreateDocumentQuery<DocumentDbMantleUser>(documentCollectionUri)
                    .Where(d => (d.UserName == name))
                    .AsEnumerable()
                    .FirstOrDefault());

            if (documentDbUser == null)
                return null;

            return mapperConfiguration.CreateMapper().Map<MantleUser>(documentDbUser);
        }

        public MantleUser FindUserByLogin(UserLoginInfo loginInfo)
        {
            loginInfo.Require(nameof(loginInfo));

            return FindUserByLoginAsync(loginInfo).Result;
        }

        public async Task<MantleUser> FindUserByLoginAsync(UserLoginInfo loginInfo)
        {
            loginInfo.Require(nameof(loginInfo));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = transientFaultStrategy.Try(
                () => DocumentClient.CreateDocumentQuery<DocumentDbMantleUser>(documentCollectionUri)
                    .SelectMany(u => (u.Logins.Where(l => (l.LoginProvider == loginInfo.LoginProvider) &&
                                                          (l.ProviderKey == loginInfo.ProviderKey)))
                                    .Select(l => u))
                    .FirstOrDefault());

            if (documentDbUser == null)
                return null;

            return mapperConfiguration.CreateMapper().Map<MantleUser>(documentDbUser);
        }

        public void UpdateUser(MantleUser user)
        {
            user.Require(nameof(user));

            UpdateUserAsync(user).Wait();
        }

        public async Task UpdateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            await EnsureDocumentCollectionExists();

            var documentCollectionUri =
                UriFactory.CreateDocumentCollectionUri(DocumentDbDatabaseId, DocumentDbCollectionId);

            var documentDbUser = mapperConfiguration.CreateMapper().Map<DocumentDbMantleUser>(user);

            await transientFaultStrategy.Try(
                () => DocumentClient.UpsertDocumentAsync(documentCollectionUri, documentDbUser));
        }

        private DocumentClient GetDocumentClient()
        {
            return (documentClient = (documentClient ??
                                      new DocumentClient(new Uri(DocumentDbEndpointUrl), DocumentDbAuthKey)));
        }

        private async Task EnsureDatabaseExists()
        {
            if (doesDbExist == false)
            {
                if (transientFaultStrategy.Try(() => DocumentClient.CreateDatabaseQuery()
                    .Where(db => (db.Id == DocumentDbDatabaseId))
                    .AsEnumerable()
                    .None()))
                {
                    if (AutoSetup)
                    {
                        var database = new Database {Id = DocumentDbDatabaseId};

                        await transientFaultStrategy.Try(() => DocumentClient.CreateDatabaseAsync(database));
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"DocumentDb database [{DocumentDbDatabaseId}] does not exist.");
                    }
                }

                doesDbExist = true;
            }
        }

        private async Task EnsureDocumentCollectionExists()
        {
            if (doesCollectionExist == false)
            {
                await EnsureDatabaseExists();

                var databaseUri = UriFactory.CreateDatabaseUri(DocumentDbDatabaseId);

                if (transientFaultStrategy.Try(() => DocumentClient.CreateDocumentCollectionQuery(databaseUri)
                    .Where(dc => (dc.Id == DocumentDbCollectionId))
                    .AsEnumerable()
                    .None()))
                {
                    if (AutoSetup)
                    {
                        var documentCollection = new DocumentCollection {Id = DocumentDbCollectionId};

                        await transientFaultStrategy.Try(
                            () => DocumentClient.CreateDocumentCollectionAsync(databaseUri, documentCollection));
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"DocumentDb document collection [{DocumentDbDatabaseId}/{DocumentDbCollectionId}] does not exist.");
                    }
                }

                doesCollectionExist = true;
            }
        }
    }
}