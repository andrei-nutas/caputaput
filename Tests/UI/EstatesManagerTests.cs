using TAF_iSAMS.Pages.Modules.SchoolManager;
using TAF_iSAMS.Pages.Extensions;
using Microsoft.Playwright;
using TAF_iSAMS.Pages.Modules.EstatesManager; // Required for Expect()

namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    internal class EstatesManagerTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_New_Building()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var estatesManagerPage = await homePage.NavigateToModuleAsync<EstatesManagerFunctions>();
            //await estatesManagerPage.NavigateToTabAsync("Forms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            EstatesManagerPopupFunctions createBuilding = await estatesManagerPage.CreateSchoolBuilding();
            estatesManagerPage = await createBuilding.EnterSchoolBuildingDetails();
            
            //Allow some time for the page to load before checking the form
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdBuilding = await estatesManagerPage.GetCreatedBuilding();
            Assert.That(createdBuilding, Is.Not.Null, $"Form found");

            //Teardown
            estatesManagerPage.RemoveCreatedBuilding();
        }
        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_New_Classroom()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var estatesManagerPage = await homePage.NavigateToModuleAsync<EstatesManagerFunctions>();
            //await estatesManagerPage.NavigateToTabAsync("Forms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            EstatesManagerPopupFunctions createClassroom = await estatesManagerPage.CreateSchoolClassroom();
            estatesManagerPage = await createClassroom.EnterSchoolClassroomDetails();

            //Allow some time for the page to load before checking the form
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdBuilding = await estatesManagerPage.GetCreatedClassroom();
            Assert.That(createdBuilding, Is.Not.Null, $"Classroom found");

            //Teardown
            estatesManagerPage.RemoveCreatedClassroom();
        }

    }
}
