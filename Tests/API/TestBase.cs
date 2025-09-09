using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.TestData.Providers;

namespace TAF_iSAMS.Tests.API
{
    /// <summary>
    /// Base class for all API tests
    /// </summary>
    public abstract class TestBase
    {
        protected ApiConfig ApiConfig;
        protected OAuthHandler OAuthHandler;
        protected SchoolDataProvider.TestData TestData;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            // Get the API configuration and test data
            ApiConfig = TestConfigProvider.GetApiConfig();
            TestData = SchoolDataProvider.GetTestData();

            // Create the OAuth handler
            OAuthHandler = new OAuthHandler(ApiConfig);
        }

        [SetUp]
        public virtual void SetUp()
        {
            // Log the test name
            TestContext.WriteLine($"Starting test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public virtual void TearDown()
        {
            // Log the test result
            var result = TestContext.CurrentContext.Result;
            TestContext.WriteLine($"Test finished: {result.Outcome.Status}");

            if (!result.Outcome.Status.Equals("Passed"))
            {
                TestContext.WriteLine($"Error message: {result.Message}");
            }
        }
    }
}