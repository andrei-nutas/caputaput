using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("YearGroups")]
    public class YearGroupTests : TestBase
    {
        private YearGroupService _yearGroupService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _yearGroupService = new YearGroupService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all year groups")]
        public async Task CanGetAllYearGroups()
        {
            // Act
            var yearGroups = await _yearGroupService.GetAllYearGroupsAsync();

            // Assert
            Assert.That(yearGroups, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {yearGroups.Count} year groups");
            foreach (var yearGroup in yearGroups.Take(5))
            {
                TestContext.WriteLine($"- {yearGroup.Name} (NC Year: {yearGroup.NcYear})");
            }
            if (yearGroups.Count > 5)
            {
                TestContext.WriteLine($"- ... and {yearGroups.Count - 5} more");
            }
        }

        [Test]
        [Description("Test getting a year group by NC year")]
        public async Task CanGetYearGroupByNcYear()
        {
            // Arrange - Use 3 as sample NC year from the Postman collection
            var ncYear = 3;

            // Act
            var yearGroup = await _yearGroupService.GetYearGroupByNcYearAsync(ncYear);

            // Assert
            Assert.That(yearGroup, Is.Not.Null);
            Assert.That(yearGroup.NcYear, Is.EqualTo(ncYear));

            // Log the results
            TestContext.WriteLine($"Retrieved year group: {yearGroup.Name} (NC Year: {yearGroup.NcYear})");
        }

        [Test]
        [Description("Test getting a year group that does not exist")]
        public void CannotGetNonExistingYearGroup()
        {
            // Arrange
            var ncYear = -100; // Non-existent NC year

            // Act & Assert
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _yearGroupService.GetYearGroupByNcYearAsync(ncYear));

            Assert.That(ex.Message, Does.Contain("404"), "Expected a 404 Not Found response");
        }
    }
}