using Mantle.Providers;
using NUnit.Framework;

namespace Mantle.Tests
{
    [TestFixture]
    public class FullNameTypeTokenProviderTests
    {
        [Test]
        public void Should_get_type_token()
        {
            var tokenProvider = new FullNameTypeTokenProvider();
            Assert.AreEqual(tokenProvider.GetTypeToken<string>(), (typeof (string).FullName));
        }
    }
}