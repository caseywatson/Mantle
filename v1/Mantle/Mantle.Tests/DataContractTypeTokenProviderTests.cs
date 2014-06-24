using System;
using System.Runtime.Serialization;
using Mantle.Providers;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class DataContractTypeTokenProviderTests
    {
        public const string TestClassName = "TestClass";
        public const string TestClassNamespace = "http://mantle.net/tests";

        public class TestClassWithoutDataContractAttribute
        {
        }

        [DataContract]
        public class TestClassWithDataContractAttribute
        {
        }

        [DataContract(Name = TestClassName)]
        public class TestClassWithDataContractAttributeWithName
        {
        }

        [DataContract(Name = TestClassName, Namespace = TestClassNamespace)]
        public class TestClassWithDataContractAttributeWithNameAndNamespace
        {
        }

        [Test]
        public void Should_get_data_token_if_DataContractAttribute_is_found()
        {
            var tokenProvider = new DataContractTypeTokenProvider();

            Assert.AreEqual(
                tokenProvider.GetTypeToken<TestClassWithDataContractAttribute>(),
                (typeof (TestClassWithDataContractAttribute).Name));
        }

        [Test]
        public void Should_get_data_token_if_DataContractAttribute_is_found_with_name()
        {
            var tokenProvider = new DataContractTypeTokenProvider();
            Assert.AreEqual(tokenProvider.GetTypeToken<TestClassWithDataContractAttributeWithName>(), TestClassName);
        }

        [Test]
        public void Should_get_data_token_if_DataContractAttribute_is_found_with_name_and_namespace()
        {
            var tokenProvider = new DataContractTypeTokenProvider();

            Assert.AreEqual(
                tokenProvider.GetTypeToken<TestClassWithDataContractAttributeWithNameAndNamespace>(),
                String.Format("{0}|{1}", TestClassNamespace, TestClassName));
        }

        [Test]
        public void Should_get_null_data_token_if_DataContractAttribute_is_not_found_on_target_type()
        {
            var tokenProvider = new DataContractTypeTokenProvider();
            Assert.IsNull(tokenProvider.GetTypeToken<TestClassWithoutDataContractAttribute>());
        }
    }
}