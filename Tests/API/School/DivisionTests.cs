using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("Divisions")]
    public class DivisionTests : TestBase
    {
        private DivisionService _divisionService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _divisionService = new DivisionService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all divisions")]
        public async Task CanGetAllDivisions()
        {
            // Act
            var divisions = await _divisionService.GetAllDivisionsAsync();

            // Assert
            Assert.That(divisions, Is.Not.Null);
            Assert.That(divisions.Count, Is.GreaterThan(0), "Should return at least one division");

            // Log the results
            TestContext.WriteLine($"Retrieved {divisions.Count} divisions");
            foreach (var division in divisions)
            {
                TestContext.WriteLine($"- {division.Name} ({division.Code})");
            }
        }

        [Test]
        [Description("Test getting a division by code")]
        public async Task CanGetDivisionByCode()
        {
            // Arrange - Use "bbn" as sample division code from the Postman collection
            var divisionCode = "bbn";

            // Act
            var division = await _divisionService.GetDivisionByCodeAsync(divisionCode);

            // Assert
            Assert.That(division, Is.Not.Null);
            Assert.That(division.Code, Is.EqualTo(divisionCode));

            // Log the results
            TestContext.WriteLine($"Retrieved division: {division.Name} ({division.Code})");
        }

        [Test]
        [Description("Test getting a division that does not exist")]
        public void CannotGetNonExistingDivision()
        {
            // Arrange
            var divisionCode = "ABC123"; // Non-existent code

            // Act & Assert
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _divisionService.GetDivisionByCodeAsync(divisionCode));

            Assert.That(ex.Message, Does.Contain("404"), "Expected a 404 Not Found response");
        }
    }
}