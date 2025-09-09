using TAF_iSAMS.Test.API.Auth;

namespace TAF_iSAMS.Tests.API.Auth
{
    [TestFixture]
    [Category("Authentication")]
    public class AuthenticationTests : TestBase
    {
        [Test]
        [Description("Test that a valid bearer token can be obtained")]
        public async Task CanGetBearerToken()
        {
            // Act
            var token = await OAuthHandler.GetBearerTokenAsync();

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
            TestContext.WriteLine($"Successfully obtained token: {token.Substring(0, 10)}...");
        }

        [Test]
        [Description("Test that the same token is returned for subsequent calls")]
        public async Task GetBearerToken_ReturnsSameToken_ForSubsequentCalls()
        {
            // Act
            var token1 = await OAuthHandler.GetBearerTokenAsync();
            var token2 = await OAuthHandler.GetBearerTokenAsync();

            // Assert
            Assert.That(token1, Is.Not.Null);
            Assert.That(token2, Is.Not.Null);
            Assert.That(token1, Is.EqualTo(token2), "Tokens should be the same for subsequent calls within expiry period");
        }

        [Test]
        [Description("Test that the OAuth troubleshooter works directly")]
        public async Task OAuthTroubleshooterWorks()
        {
            // Create the OAuth troubleshooter using the same configuration as the OAuthHandler
            var troubleshooter = new OAuthTroubleshooter();

            // Run the troubleshooter
            await troubleshooter.DirectOAuthTest();

            // The test will fail if any assertions in DirectOAuthTest fail
            Assert.Pass("OAuth troubleshooter completed successfully");
        }
    }
}