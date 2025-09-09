using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("Terms")]
    public class TermTests : TestBase
    {
        private TermService _termService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _termService = new TermService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all terms")]
        public async Task CanGetAllTerms()
        {
            // Act
            var terms = await _termService.GetAllTermsAsync();

            // Assert
            Assert.That(terms, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {terms.Count} terms");
            foreach (var term in terms.Take(5))
            {
                TestContext.WriteLine($"- {term.Name} ({term.AcademicYear}): {term.StartDate:yyyy-MM-dd} to {term.EndDate:yyyy-MM-dd}");
            }
            if (terms.Count > 5)
            {
                TestContext.WriteLine($"- ... and {terms.Count - 5} more");
            }
        }
    }
}