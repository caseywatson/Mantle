using Mantle.Providers;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class NameTypeTokenProviderTests
    {
        [Test]
        public void Should_get_type_token()
        {
            var tokenProvider = new NameTypeTokenProvider();
            Assert.AreEqual(tokenProvider.GetTypeToken<string>(), (typeof (string).Name));
        }
    }
}