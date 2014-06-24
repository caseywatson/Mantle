using Mantle.Providers;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class AssemblyQualifiedNameTypeTokenProviderTests
    {
        [Test]
        public void Should_get_type_token()
        {
            var tokenProvider = new AssemblyQualifiedNameTypeTokenProvider();

            Assert.AreEqual(tokenProvider.GetTypeToken<string>(), (typeof (string).AssemblyQualifiedName));
        }
    }
}