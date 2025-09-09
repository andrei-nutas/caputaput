using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("Houses")]
    public class HouseTests : TestBase
    {
        private HouseService _houseService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _houseService = new HouseService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all houses")]
        public async Task CanGetAllHouses()
        {
            // Act
            var houses = await _houseService.GetAllHousesAsync();

            // Assert
            Assert.That(houses, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {houses.Count} houses");
            foreach (var house in houses.Take(5))
            {
                TestContext.WriteLine($"- {house.Name} ({house.Code})");
            }
            if (houses.Count > 5)
            {
                TestContext.WriteLine($"- ... and {houses.Count - 5} more");
            }
        }

        [Test]
        [Description("Test getting academic houses")]
        public async Task CanGetAcademicHouses()
        {
            // Act
            var houses = await _houseService.GetHousesByTypeAsync("academic");

            // Assert
            Assert.That(houses, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {houses.Count} academic houses");
        }

        [Test]
        [Description("Test getting boarding houses")]
        public async Task CanGetBoardingHouses()
        {
            // Act
            var houses = await _houseService.GetHousesByTypeAsync("boarding");

            // Assert
            Assert.That(houses, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {houses.Count} boarding houses");
        }

        [Test]
        [Description("Test getting a house by ID")]
        public async Task CanGetHouseById()
        {
            // Arrange
            var houseId = int.Parse(TestData.HouseId); // Parse as int

            // Act
            var house = await _houseService.GetHouseByIdAsync(houseId);

            // Assert
            Assert.That(house, Is.Not.Null);
            Assert.That(house.Id, Is.EqualTo(houseId));

            // Log the results
            TestContext.WriteLine($"Retrieved house: {house.Name} ({house.Code})");
        }

        [Test]
        [Description("Test getting a house that does not exist")]
        public void CannotGetNonExistingHouse()
        {
            // Arrange
            var houseId = -1; // Non-existent ID as int

            // Act & Assert
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _houseService.GetHouseByIdAsync(houseId));

            Assert.That(ex.Message, Does.Contain("404"), "Expected a 404 Not Found response");
        }
    }
}