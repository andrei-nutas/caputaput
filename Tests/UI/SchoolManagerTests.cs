using Microsoft.Playwright; // Required for Expect()
using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Pages.Modules.SchoolManager;

namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    internal class SchoolManagerTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        public async Task Test_SchoolSetup_Configuration() // Renamed for clarity
        {
            // Arrange
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();

            // Act
            await schoolManagerPage.NavigateToTabAsync("School Setup"); // Navigate to correct tab

            // Interact with elements on the School Setup tab
            await schoolManagerPage.UpdateSchoolDetails();

            TestContext.WriteLine("School Setup tab interactions completed.");
            // Add more specific assertions based on expected outcomes
        }


        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_Basic_Form()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Forms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions createFormPopup = await schoolManagerPage.CreateFormPopup();
            schoolManagerPage = await createFormPopup.EnterFormDataAndReturnMainWindow();
            
            //Allow some time for the page to load before checking the form
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdForm = await schoolManagerPage.GetFormOnPage();
            Assert.That(createdForm, Is.Not.Null, $"Form found");

            //Teardown
            schoolManagerPage.DeleteCreatedSchoolForm();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_Academic_House()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Houses"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions createHousePopup = await schoolManagerPage.CreateHousePopup();
            schoolManagerPage = await createHousePopup.EnterAcademicHouseDetailsAndReturnToMainWindow();

            //Assert: Confirm the element is found on the page
            var createdHouse = await schoolManagerPage.GetHouseOnPage();
            Assert.That(createdHouse, Is.Not.Null, $"House found");

            //Teardown
            schoolManagerPage.DeleteCreatedAcademicHouse();
        }


        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Add_Tutor()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Tutors"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions createTutorPopup = await schoolManagerPage.CreateTutorPopup();
            schoolManagerPage = await createTutorPopup.SelectTutorAndReturnToMainWindow();

            //Assert: Confirm the element is found on the page
            var createdTutor = await schoolManagerPage.GetTutorOnPage();
            Assert.That(createdTutor, Is.Not.Null, $"Tutor found");

            //Teardown
            await schoolManagerPage.DeleteTutorAndConfirm();
            var deletedTutor = await schoolManagerPage.GetTutorOnPage();
            Assert.That(deletedTutor, Is.Null, $"Tutor found");
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_Year()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Years"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions createYearPopup = await schoolManagerPage.CreateYearPopup();          
            schoolManagerPage = await createYearPopup.EnterYearDataAndReturnToMainWindow();
            
            //Assert: Confirm the element is found on the page
            var createdYear = await schoolManagerPage.GetYearOnPage();
            Assert.That(createdYear, Is.Not.Null, $"Year found");

            //Teardown
            schoolManagerPage.DeleteCreatedSchoolYear();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_Year_Block()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Year Blocks"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions createYearBlockPopup = await schoolManagerPage.CreateYearBlockPopup();
            schoolManagerPage = await createYearBlockPopup.EnterYearBlockDataAndReturnToMainWindow();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdYearBlock = await schoolManagerPage.GetYearBlockOnPage();
            Assert.That(createdYearBlock, Is.Not.Null, $"Year Block found");

            //Teardown
            schoolManagerPage.DeleteCreatedYearBlock();
        }


        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_School_Term()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("School Terms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions CreateSchoolTerm = await schoolManagerPage.CreateSchoolTermPopup();
            schoolManagerPage = await CreateSchoolTerm.EnterTermDataAndReturnToMainWindow();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdYearBlock = await schoolManagerPage.GetSchoolTerm();
            Assert.That(createdYearBlock, Is.Not.Null, $"School Term found");

            //Teardown
            schoolManagerPage.DeleteCreatedSchoolTerm();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_School_Teaching_Department()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("School Departments"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions CreateSchoolTerm = await schoolManagerPage.CreateSchoolDepartmentPopup();
            schoolManagerPage = await CreateSchoolTerm.EnterTeachingDepartmentAndReturnToMainWindow();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdDepartment = await schoolManagerPage.GetSchoolDepartment();
            Assert.That(createdDepartment, Is.Not.Null, $"School Department found");

            //Teardown
            schoolManagerPage.DeleteCreatedSchoolDepartment();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Create_School_Division()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("School Divisions"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions CreateSchoolDivision = await schoolManagerPage.CreateSchoolDivisionPopup();
            schoolManagerPage = await CreateSchoolDivision.EnterDivisionDataAndReturnToMainWindow();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdDivision = await schoolManagerPage.GetSchoolDivision();
            Assert.That(createdDivision, Is.Not.Null, $"School Division found");

            //Teardown
            schoolManagerPage.DeleteCreatedSchoolDivision();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_School_Form()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Forms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions schoolForm = await schoolManagerPage.EditSchoolFormPopup();
            schoolManagerPage = await schoolForm.EditUpdateSchoolForm();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var createdDivision = await schoolManagerPage.GetSchoolFormRoom();
            Assert.That(createdDivision, Is.Not.Null, $"School Form Room found");
        }


        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_Academic_House()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Houses"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions academicHouse = await schoolManagerPage.EditAcademicHousePopup();
            schoolManagerPage = await academicHouse.EditUpdateAcademicHouse();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var updatedAcademicHouse = await schoolManagerPage.GetAcademicHouseMaster();
            Assert.That(updatedAcademicHouse, Is.Not.Null, $"Academic House Master found");
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_New_School_Department()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            schoolManagerPage.CreateTempDepartment();
            await schoolManagerPage.NavigateToTabAsync("School Departments"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions academicHouse = await schoolManagerPage.EditTempDepartmentPopup();
            schoolManagerPage = await academicHouse.EditUpdateTempDepartment();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var updatedAcademicHouse = await schoolManagerPage.GetTempSchoolDepartment();
            Assert.That(updatedAcademicHouse, Is.Not.Null, $"Temp Department found");

            schoolManagerPage.DeleteTempDepartment();
        }


        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_New_School_Division()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            schoolManagerPage.CreateTempDivision();
            await schoolManagerPage.NavigateToTabAsync("School Divisions"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions schoolDivision = await schoolManagerPage.EditTempDivisionPopup();
            schoolManagerPage = await schoolDivision.EditUpdateTempDivision();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var updatedDivision = await schoolManagerPage.GetTempSchoolDivision();
            Assert.That(updatedDivision, Is.Not.Null, $"Temp Division found");

            schoolManagerPage.DeleteTempDivision();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_School_Term()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("School Terms"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions schoolDivision = await schoolManagerPage.EditSchoolTerm();
            schoolManagerPage = await schoolDivision.EditUpdateTerm();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var updatedDivision = await schoolManagerPage.GetSchoolTerm();
            Assert.That(updatedDivision, Is.Not.Null, $"School Term found");

            schoolManagerPage.RevertSchoolTerm();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_Tutor()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Tutors"); // Navigate to correct tab

            // Act: Create the tutor
            SchoolManagerPopupFunctions createTutorPopup = await schoolManagerPage.CreateTutorPopup();
            schoolManagerPage = await createTutorPopup.SelectTutorAndReturnToMainWindow();

            //Assert:Verify tutor is visible
            var createdTutor = await schoolManagerPage.GetTutorOnPage();
            Assert.That(createdTutor, Is.Not.Null, $"Tutor found");

            //Edit the tutor
            SchoolManagerPopupFunctions editTutorPopup = await schoolManagerPage.EditTutorPopup();
            schoolManagerPage = await editTutorPopup.SelectAlternativeTutorAndReturnToMainWindow();

            //Verify the new tutor
            var alternativeTutor = await schoolManagerPage.GetAlternativeTutorOnPage();
            Assert.That(alternativeTutor, Is.Not.Null, $"Tutor found");

            //Remove the new tutor
            await schoolManagerPage.DeleteAlternativeTutorAndConfirm();

            //Ensure deletion
            var deletedTutor = await schoolManagerPage.GetAlternativeTutorOnPage();
            Assert.That(deletedTutor, Is.Null, $"Tutor not found");
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_Year_Block()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            schoolManagerPage.CreateYearBlock();
            await schoolManagerPage.NavigateToTabAsync("Year Blocks"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions yearBlock = await schoolManagerPage.EditYearBlock();
            schoolManagerPage = await yearBlock.EditCreatedYearBlock();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);
            //Assert: Confirm the element is found on the page
            var updatedYearBlock = await schoolManagerPage.GetUpdatedYearBlockOnPage();
            Assert.That(updatedYearBlock, Is.Not.Null, $"Year Block found");

            schoolManagerPage.DeleteUpdatedYearBlock();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Edit_Year()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            schoolManagerPage.CreateYear();
            await schoolManagerPage.NavigateToTabAsync("Years"); // Navigate to correct tab

            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions year = await schoolManagerPage.EditYear();
            schoolManagerPage = await year.EditCreatedYear();

            // A small delay is needed here as we return to the main page from the popup.   
            await Task.Delay(500);

            //Assert: Confirm the element is found on the page
            var updatedYear = await schoolManagerPage.GetUpdatedYear();
            Assert.That(updatedYear, Is.Not.Null, $"Year Block found");

            schoolManagerPage.DeleteCreatedYear();
        }

        [Test]
        [Category("AutoLogin")]
        [Category("SchoolManager")]
        [Category("PopupHandling")] // Add a category for tests involving popups
        public async Task Test_Add_Pupil_To_Form()
        {
            // Arrange: Navigate to the starting state
            await homePage.ClickViewAllModulesAsync();
            var schoolManagerPage = await homePage.NavigateToModuleAsync<SchoolManagerFunctions>();
            await schoolManagerPage.NavigateToTabAsync("Forms"); // Navigate to correct tab



            // Act: Perform the workflow involving the popup
            SchoolManagerPopupFunctions form = await schoolManagerPage.EditFormPupils();

            schoolManagerPage = await form.AddPupilsToForm(); //Test fails here

            // A small delay is needed here as we return to the main page from the popup.   
             await Task.Delay(500);

            //Assert: Confirm the element is found on the page
            var updatedYear = await schoolManagerPage.GetUpdatedYear();
            Assert.That(updatedYear, Is.Not.Null, $"Year Block found");

            // schoolManagerPage.DeleteCreatedYear();
        }
    }
}
