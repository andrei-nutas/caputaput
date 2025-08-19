using TAF_iSAMS.Pages.Modules.HumanResourcesManager;
using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    // Optionally you can run tests within this fixture in parallel:
    [Parallelizable(ParallelScope.Children)]
    public class HumanResourcesTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("Human Resources")]
        public async Task Test_Add_New_Staff_Member()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var humanResourcesPage = await homePage.NavigateToModuleAsync<HumanResourcesManagerFunctions>();

            // 4) In the top iFrame, click on "Add Staff"
            //    We'll use your ModuleNavigationBar class to do that:
            await humanResourcesPage.NavigateToTabAsync("Add Staff");

            // 5) Delete any existing staff member which may have been created in a previous run of this test but was never deleted (e.g the test crashed before reaching the delete staff member step)
            humanResourcesPage.DeleteStaffMember();

            // 6) Enter the required staff data on the "Add Staff" tab
            await humanResourcesPage.EnterStaffDetails();

            // 7) Click the "Next Step" on the "Add Staff" tab
            await humanResourcesPage.ExecuteNextStepButton();

            // 8) Assertion
            var newStaffSuccessMessageVisible = await humanResourcesPage.NewStaffCreatedMessageVisible();
            Assert.That(newStaffSuccessMessageVisible, Is.True, "New staff member added successfully");

            // 9) Clean up data - delete the newly created staff member to ensure the test is re-usable
            humanResourcesPage.DeleteStaffMember();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Human Resources")]
        public async Task Test_Search_Staff_Member()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var humanResourcesPage = await homePage.NavigateToModuleAsync<HumanResourcesManagerFunctions>();

            // 4) In the top iFrame, click on "Add Staff"
            //    We'll use your ModuleNavigationBar class to do that:
            await humanResourcesPage.NavigateToTabAsync("Manage Staff");

            // 5) Add a staff member directly to the database to ensure the staff member is available for searching in the test
            humanResourcesPage.DeleteStaffSearchMember();
            humanResourcesPage.AddStaffSearchMember();

            // 6) Enter the staff member surname and forename within the search page on the Manager Staff tab
            await humanResourcesPage.EnterStaffSearchDetails();

            // 7) Click the search button to return staff member search results
            await humanResourcesPage.ExecuteSearchButton();

            // 8) Click the search button to return staff member search results
            await humanResourcesPage.ClickStaffNameOnPage();

            // 9) Assertion
            var staffRecordOpened = await humanResourcesPage.StaffEnrolmentTabVisible();
            Assert.That(staffRecordOpened, Is.True, "Enrolment tab found");

            // 10) Data clean up - delete the staff member created specifically for this to make the test re-usable
            humanResourcesPage.DeleteStaffSearchMember();

        }



        [Test]
        [Category("AutoLogin")]
        [Category("Human Resources")]
        public async Task Edit_Single_Staff_Member()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var humanResourcesPage = await homePage.NavigateToModuleAsync<HumanResourcesManagerFunctions>();

            // 4) In the top iFrame, click on "Add Staff"
            //    We'll use your ModuleNavigationBar class to do that:
            await humanResourcesPage.NavigateToTabAsync("Manage Staff");

            // 5) Add a staff member directly to the database to ensure the staff member is available for searching in the test
            humanResourcesPage.AddStaffSearchMember();

            // 6) Enter the staff member surname and forename within the search page on the Manager Staff tab
            await humanResourcesPage.EnterStaffSearchDetails();

            // 7) Click the search button to return staff member search results
            await humanResourcesPage.ExecuteSearchButton();

            // 8) Click the staff member that is returned in the search results
            await humanResourcesPage.ClickStaffNameOnPage();

            // 9) Amend the 'School Initials' field for the staff member on the 'General' page
            await humanResourcesPage.UpdateStaffSchoolInitialsField();

            // 10) Click the 'Update Data' button to save the 'School Initials' changes made in the previous step
            await humanResourcesPage.ClickUpdateDataButton();

            // 11) Assertion - check staff updated message appears on the page
            await humanResourcesPage.StaffUpdatedMessageVisible();

            // 12) Data clean up - delete the staff member created specifically for this to make the test re-usable
            humanResourcesPage.DeleteEditedStaffMember();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Human Resources")]
        public async Task Edit_Multiple_Staff_Members()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var humanResourcesPage = await homePage.NavigateToModuleAsync<HumanResourcesManagerFunctions>();

            // 4) In the top iFrame, click on "Add Staff"
            //    We'll use your ModuleNavigationBar class to do that:
            await humanResourcesPage.NavigateToTabAsync("Manage Staff");

            // 5) Add two staff members directly to the database to ensure the staff members are available for searching in the test
            humanResourcesPage.AddStaffSearchMember();
            humanResourcesPage.AddSecondStaffMember();

            // 6) Enter the staff member surname and forename within the search page on the Manager Staff tab
            await humanResourcesPage.EnterStaffSearchDetails();

            // 7) Click the search button to return staff member search results
            await humanResourcesPage.ExecuteSearchButton();

            // 8) Tick the 'Select all' check box to highlight both staff members in the search results
            await humanResourcesPage.TickAllSearchResults();

            // 9) Select Group Edit from the pink drop down and update the School Initials field for both members of staff in the subsequent pop up window.
            // After performing the School Initials data change and saving, the test proceeds to close the pop up and return to the main window.
            HumanResourcesManagerPopupFunctions createGroupEditPopup = await humanResourcesPage.CreateGroupEditPopup();
            humanResourcesPage = await createGroupEditPopup.SelectFieldAndReturnToMainWindow();

        }


    }
}
