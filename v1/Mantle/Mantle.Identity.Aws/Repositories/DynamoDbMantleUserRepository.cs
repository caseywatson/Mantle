using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Identity.Interfaces;
using Microsoft.AspNet.Identity;

namespace Mantle.Identity.Aws.Repositories
{
    public class DynamoDbMantleUserRepository : IMantleUserRepository<MantleUser>, IDisposable
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private AmazonDynamoDBClient dynamoDbClient;

        public DynamoDbMantleUserRepository(IAwsRegionEndpoints awsRegionEndpoints)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;

            AutoSetup = true;
            TableName = "MantleUsers";
            TableReadCapacityUnits = 5;
            TableWriteCapacityUnits = 5;
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

        public AmazonDynamoDBClient AmazonDynamoDbClient => GetAmazonDynamoDbClient();

        public void Dispose()
        {
            dynamoDbClient?.Dispose();
        }

        public void CreateUser(MantleUser user)
        {
            user.Require(nameof(user));

            transientFaultStrategy.Try(() => AmazonDynamoDbClient.PutItem(TableName, ToDocumentDictionary(user)));
        }

        public async Task CreateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            await AmazonDynamoDbClient.PutItemAsync(TableName, ToDocumentDictionary(user));
        }

        public void DeleteUser(string userId)
        {
            userId.Require(nameof(userId));

            transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.DeleteItem(TableName, new Dictionary<string, AttributeValue>
                {
                    [nameof(MantleUser.Id)] = new AttributeValue {S = userId}
                }));
        }

        public async Task DeleteUserAsync(string userId)
        {
            userId.Require(nameof(userId));

            await (transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.DeleteItemAsync(TableName, new Dictionary<string, AttributeValue>
                {
                    [nameof(MantleUser.Id)] = new AttributeValue {S = userId}
                })));
        }

        public MantleUser FindUserByEmail(string email)
        {
            const string emailParameter = ":email";

            email.Require(nameof(email));

            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = $"{nameof(MantleUser.Email)}SecondaryIndex",
                ScanIndexForward = true,
                KeyConditionExpression = $"{nameof(MantleUser.Email)} = {emailParameter}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [emailParameter] = new AttributeValue {S = email}
                },
                Select = Select.ALL_PROJECTED_ATTRIBUTES
            };

            var userDocDictionary = transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.Query(queryRequest).Items?.FirstOrDefault());

            if (userDocDictionary == null)
                return null;

            return ToMantleUser(userDocDictionary);
        }

        public async Task<MantleUser> FindUserByEmailAsync(string email)
        {
            const string emailParameter = ":email";

            email.Require(nameof(email));

            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = $"{nameof(MantleUser.Email)}SecondaryIndex",
                ScanIndexForward = true,
                KeyConditionExpression = $"{nameof(MantleUser.Email)} = {emailParameter}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [emailParameter] = new AttributeValue {S = email}
                },
                Select = Select.ALL_PROJECTED_ATTRIBUTES
            };

            var queryResponse = await (transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.QueryAsync(queryRequest)));

            var userDocDictionary = queryResponse.Items?.FirstOrDefault();

            if (userDocDictionary == null)
                return null;

            return ToMantleUser(userDocDictionary);
        }

        public MantleUser FindUserById(string id)
        {
            id.Require(nameof(id));

            var getItemResult = transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.GetItem(TableName, new Dictionary<string, AttributeValue>
                {
                    [nameof(MantleUser.Id)] = new AttributeValue {S = id}
                }));

            if (getItemResult.IsItemSet)
                return ToMantleUser(getItemResult.Item);

            return null;
        }

        public async Task<MantleUser> FindUserByIdAsync(string id)
        {
            id.Require(nameof(id));

            var getItemResult = await (transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.GetItemAsync(TableName, new Dictionary<string, AttributeValue>
                {
                    [nameof(MantleUser.Id)] = new AttributeValue {S = id}
                })));

            if (getItemResult.IsItemSet)
                return ToMantleUser(getItemResult.Item);

            return null;
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
            const string userNameParameter = ":userName";

            name.Require(nameof(name));

            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = $"{nameof(MantleUser.UserName)}SecondaryIndex",
                ScanIndexForward = true,
                KeyConditionExpression = $"{nameof(MantleUser.UserName)} = {userNameParameter}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [userNameParameter] = new AttributeValue {S = name}
                },
                Select = Select.ALL_PROJECTED_ATTRIBUTES
            };

            var userDocDictionary = transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.Query(queryRequest).Items?.FirstOrDefault());

            if (userDocDictionary == null)
                return null;

            return ToMantleUser(userDocDictionary);
        }

        public async Task<MantleUser> FindUserByNameAsync(string name)
        {
            const string userNameParameter = ":userName";

            name.Require(nameof(name));

            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                IndexName = $"{nameof(MantleUser.UserName)}SecondaryIndex",
                ScanIndexForward = true,
                KeyConditionExpression = $"{nameof(MantleUser.UserName)} = {userNameParameter}",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    [userNameParameter] = new AttributeValue {S = name}
                },
                Select = Select.ALL_PROJECTED_ATTRIBUTES
            };

            var queryResponse = await (transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.QueryAsync(queryRequest)));

            var userDocDictionary = queryResponse.Items?.FirstOrDefault();

            if (userDocDictionary == null)
                return null;

            return ToMantleUser(userDocDictionary);
        }

        public void UpdateUser(MantleUser user)
        {
            user.Require(nameof(user));

            transientFaultStrategy.Try(() => AmazonDynamoDbClient.PutItem(TableName, ToDocumentDictionary(user)));
        }

        public async Task UpdateUserAsync(MantleUser user)
        {
            user.Require(nameof(user));

            await (transientFaultStrategy.Try(
                () => AmazonDynamoDbClient.PutItemAsync(TableName, ToDocumentDictionary(user))));
        }

        private Dictionary<string, AttributeValue> ToDocumentDictionary(MantleUser user)
        {
            var docDictionary = new Dictionary<string, AttributeValue>
            {
                [nameof(user.Id)] = ToAttributeValue(user.Id),
                [nameof(user.EmailConfirmed)] = new AttributeValue {BOOL = user.EmailConfirmed},
                [nameof(user.LockoutEnabled)] = new AttributeValue {BOOL = user.LockoutEnabled},
                [nameof(user.PhoneNumberConfirmed)] = new AttributeValue {BOOL = user.PhoneNumberConfirmed},
                [nameof(user.TwoFactorEnabled)] = new AttributeValue {BOOL = user.TwoFactorEnabled},
                [nameof(user.LockoutEndDate)] = new AttributeValue {S = user.LockoutEndDate.ToString("o")},
                [nameof(user.AccessFailedCount)] = new AttributeValue {N = user.AccessFailedCount.ToString()},
                [nameof(user.Email)] = ToAttributeValue(user.Email),
                [nameof(user.PasswordHash)] = ToAttributeValue(user.PasswordHash),
                [nameof(user.PhoneNumber)] = ToAttributeValue(user.PhoneNumber),
                [nameof(user.SecurityStamp)] = ToAttributeValue(user.SecurityStamp),
                [nameof(user.UserName)] = ToAttributeValue(user.UserName)
            };

            if (user.Claims.None())
                docDictionary[nameof(user.Claims)] = new AttributeValue {NULL = true};
            else
            {
                docDictionary[nameof(user.Claims)] = new AttributeValue
                {
                    L = user.Claims.Select(c => new AttributeValue {M = ToDocumentDictionary(c)}).ToList()
                };
            }

            if (user.Logins.None())
                docDictionary[nameof(user.Logins)] = new AttributeValue {NULL = true};
            else
            {
                docDictionary[nameof(user.Logins)] = new AttributeValue
                {
                    L = user.Logins.Select(l => new AttributeValue {M = ToDocumentDictionary(l)}).ToList()
                };
            }

            if (user.Roles.None())
                docDictionary[nameof(user.Roles)] = new AttributeValue {NULL = true};
            else
                docDictionary[nameof(user.Roles)] = new AttributeValue {SS = user.Roles};

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

            user.LockoutEndDate = DateTimeOffset.Parse(docDictionary[nameof(user.LockoutEndDate)].S);

            user.AccessFailedCount = int.Parse(docDictionary[nameof(user.AccessFailedCount)].N);

            if (docDictionary[nameof(user.Claims)].NULL == false)
                user.Claims = docDictionary[nameof(user.Claims)].L.Select(av => ToMantleUserClaim(av.M)).ToList();

            if (docDictionary[nameof(user.Logins)].NULL == false)
                user.Logins = docDictionary[nameof(user.Logins)].L.Select(av => ToMantleUserLogin(av.M)).ToList();

            if (docDictionary[nameof(user.Roles)].NULL == false)
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
            return new Dictionary<string, AttributeValue>
            {
                [nameof(claim.Id)] = ToAttributeValue(claim.Id),
                [nameof(claim.UserId)] = ToAttributeValue(claim.UserId),
                [nameof(claim.ClaimType)] = ToAttributeValue(claim.ClaimType),
                [nameof(claim.ClaimValue)] = ToAttributeValue(claim.ClaimValue)
            };
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
            return new Dictionary<string, AttributeValue>
            {
                [nameof(login.Id)] = ToAttributeValue(login.Id),
                [nameof(login.UserId)] = ToAttributeValue(login.UserId),
                [nameof(login.LoginProvider)] = ToAttributeValue(login.LoginProvider),
                [nameof(login.ProviderKey)] = ToAttributeValue(login.ProviderKey)
            };
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

        private AttributeValue ToAttributeValue(string source)
        {
            return (string.IsNullOrEmpty(source)
                ? new AttributeValue {NULL = true}
                : new AttributeValue {S = source});
        }

        private AmazonDynamoDBClient GetAmazonDynamoDbClient()
        {
            if (dynamoDbClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a knnown AWS region.");

                dynamoDbClient = transientFaultStrategy.Try(
                    () => new AmazonDynamoDBClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint));

                if (AutoSetup)
                    SetupTable(dynamoDbClient);
            }

            return dynamoDbClient;
        }

        private void SetupTable(AmazonDynamoDBClient dynamoDbClient)
        {
            if (transientFaultStrategy.Try(() => DoesTableExist(dynamoDbClient)) == false)
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

                transientFaultStrategy.Try(() => dynamoDbClient.CreateTable(createTableRequest));
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
                if (transientFaultStrategy.Try(() => DoesTableExist(dynamoDbClient)))
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