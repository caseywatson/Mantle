using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Threading;

namespace Mantle.Identity.Aws.Repositories
{
    public class DynamoDbMantleUserRepository : IMantleUserRepository<MantleUser>, IDisposable
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;

        private AmazonDynamoDBClient dynamoDbClient;

        public DynamoDbMantleUserRepository(IAwsRegionEndpoints awsRegionEndpoints)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;

            AutoSetup = true;
            TableName = "MantleUsers";
            TableReadCapacityUnits = 10;
            TableWriteCapacityUnits = 10;
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsRegionName { get; set; }

        [Configurable]
        public string TableName { get; set; }

        [Configurable]
        public int TableReadCapacityUnits { get; set; }

        [Configurable]
        public int TableWriteCapacityUnits { get; set; }

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
            user.Logins = docDictionary[nameof(user.Logins)].L.Select(av => ToMantleUserLogin(av.M)).ToList();
            user.Roles = docDictionary[nameof(user.Roles)].SS;

            user.Email = docDictionary[nameof(user.Email)].S;
            user.PasswordHash = docDictionary[nameof(user.PasswordHash)].S;
            user.PhoneNumber = docDictionary[nameof(user.PhoneNumber)].S;
            user.SecurityStamp = docDictionary[nameof(user.SecurityStamp)].S;
            user.UserName = docDictionary[nameof(user.UserName)].S;

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

        private MantleUserLogin ToMantleUserLogin(Dictionary<string, AttributeValue> docDictionary)
        {
            var login = new MantleUserLogin();

            login.Id = docDictionary[nameof(login.Id)].S;
            login.UserId = docDictionary[nameof(login.UserId)].S;
            login.LoginProvider = docDictionary[nameof(login.LoginProvider)].S;
            login.ProviderKey = docDictionary[nameof(login.ProviderKey)].S;

            return login;
        }

        private AmazonDynamoDBClient GetAmazonDynamoDbClient()
        {
            if (dynamoDbClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a knnown AWS region.");

                dynamoDbClient = new AmazonDynamoDBClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint);

                if (AutoSetup)
                    SetupTable(dynamoDbClient);
            }

            return dynamoDbClient;
        }

        private void SetupTable(AmazonDynamoDBClient dynamoDbClient)
        {
            if (DoesTableExist(dynamoDbClient) == false)
            {
                var createTableRequest = new CreateTableRequest
                {
                    TableName = TableName,
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = TableReadCapacityUnits,
                        WriteCapacityUnits = TableWriteCapacityUnits
                    },
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = nameof(MantleUser.Id),
                            AttributeType = ScalarAttributeType.S
                        },
                        new AttributeDefinition
                        {
                            AttributeName = nameof(MantleUser.Email),
                            AttributeType = ScalarAttributeType.S
                        },
                        new AttributeDefinition
                        {
                            AttributeName = nameof(MantleUser.UserName),
                            AttributeType = ScalarAttributeType.S
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = nameof(MantleUser.Id),
                            KeyType = KeyType.HASH
                        }
                    },
                    GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                    {
                        CreateEmailSecondaryIndex(),
                        CreateUserNameSecondaryIndex()
                    }
                };

                dynamoDbClient.CreateTable(createTableRequest);
                WaitUntilTableExists(dynamoDbClient);
            }
        }

        private GlobalSecondaryIndex CreateEmailSecondaryIndex()
        {
            return new GlobalSecondaryIndex
            {
                IndexName = $"{nameof(MantleUser.Email)}SecondaryIndex",
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = TableReadCapacityUnits,
                    WriteCapacityUnits = TableWriteCapacityUnits
                },
                Projection = new Projection {ProjectionType = ProjectionType.ALL},
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = nameof(MantleUser.Email),
                        KeyType = KeyType.HASH
                    }
                }
            };
        }

        private GlobalSecondaryIndex CreateUserNameSecondaryIndex()
        {
            return new GlobalSecondaryIndex
            {
                IndexName = $"{nameof(MantleUser.UserName)}SecondaryIndex",
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = TableReadCapacityUnits,
                    WriteCapacityUnits = TableWriteCapacityUnits
                },
                Projection = new Projection {ProjectionType = ProjectionType.ALL},
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = nameof(MantleUser.UserName),
                        KeyType = KeyType.HASH
                    }
                }
            };
        }

        private void WaitUntilTableExists(AmazonDynamoDBClient dynamoDbClient)
        {
            while (true)
            {
                if (DoesTableExist(dynamoDbClient))
                    return;

                Thread.Sleep(5000);
            }
        }

        private bool DoesTableExist(AmazonDynamoDBClient dynamoDbClient)
        {
            try
            {
                return (dynamoDbClient.DescribeTable(TableName).Table?.TableStatus == TableStatus.ACTIVE);
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }
    }
}