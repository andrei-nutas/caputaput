using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Pages.Modules.SchoolManager;
using TAF_iSAMS.Pages.Modules.TimetableManager;

namespace TAF_iSAMS.Tests.UI
{

    [TestFixture]
    // Optionally you can run tests within this fixture in parallel:
    [Parallelizable(ParallelScope.Children)]
    public class TimetableManagerTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("Timetable Manager")]
        public async Task Test_Add_Timetable_Day()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Timetable Manager" and click on the module.
            var timetableManagerPage = await homePage.NavigateToModuleAsync<TimetableManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await timetableManagerPage.NavigateToTabAsync("Configuration");

            // 5) Click on the Manage Periods and Days button
            await timetableManagerPage.ClickManagePeriodsAndDaysButton();

            // 6) Perform the workflow involving the popup
            TimetableManagerPopupFunctions createATimetableDay = await timetableManagerPage.CreateTimetableDayAndGetPopupAsync();
            timetableManagerPage = await createATimetableDay.SelectFieldAndReturnToMainWindow();

            // 7) Assertion - check that the Timetable Day has been created
            var createdTimetableDay = await timetableManagerPage.GetCreatedTimetableDay();
            Assert.That(createdTimetableDay, Is.Not.Null, $"Timetable day found");

            // 8) Test data clean up - delete the created Timetable Day to ensure the test is re-usable
            timetableManagerPage.DeleteTimetableDay();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Timetable Manager")]
        public async Task Test_Add_Timetable_Week()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Timetable Manager" and click on the module.
            var timetableManagerPage = await homePage.NavigateToModuleAsync<TimetableManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await timetableManagerPage.NavigateToTabAsync("Configuration");

            // 5) Click on the Manage Periods and Days button
            await timetableManagerPage.ClickManagePeriodsAndDaysButton();

            // 6) Perform the workflow involving the popup
            TimetableManagerPopupFunctions createATimetableWeek = await timetableManagerPage.CreateTimetableWeekAndGetPopupAsync();
            timetableManagerPage = await createATimetableWeek.SelectTimetableWeekFieldsAndReturnToMainWindow();

            // 7) Assertion - check that the Timetable Week has been created
            var createdTimetableWeek = await timetableManagerPage.GetCreatedTimetableWeek();
            Assert.That(createdTimetableWeek, Is.Not.Null, $"Timetable week found");

            // 8) Test data clean up - delete the created Timetable Week to ensure the test is re-usable
            timetableManagerPage.DeleteTimetableWeek();

        }



        [Test]
        [Category("AutoLogin")]
        [Category("Timetable Manager")]
        public async Task Test_Add_Timetable_Period()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Timetable Manager" and click on the module.
            var timetableManagerPage = await homePage.NavigateToModuleAsync<TimetableManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await timetableManagerPage.NavigateToTabAsync("Configuration");

            // 5) Click on the Manage Periods and Days button
            await timetableManagerPage.ClickManagePeriodsAndDaysButton();

            // 6) Perform the workflow involving the popup
            TimetableManagerPopupFunctions createATimetablePeriod = await timetableManagerPage.CreateTimetablePeriodAndGetPopupAsync();
            timetableManagerPage = await createATimetablePeriod.SelectTimetablePeriodFieldsAndReturnToMainWindow();

            // 7) Assertion - check that the Timetable Period has been created
            var createdTimetablePeriod = await timetableManagerPage.GetCreatedTimetablePeriod();
            Assert.That(createdTimetablePeriod, Is.Not.Null, $"Timetable period found");

            // 8) Test data clean up - delete the created Timetable Period to ensure the test is re-usable
            timetableManagerPage.DeleteTimetablePeriod();


        }



        [Test]
        [Category("AutoLogin")]
        [Category("Timetable Manager")]
        public async Task Test_Edit_Timetable_Week()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Timetable Manager" and click on the module.
            var timetableManagerPage = await homePage.NavigateToModuleAsync<TimetableManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await timetableManagerPage.NavigateToTabAsync("Configuration");

            // 5) Add a timetable week directly to the database to ensure a test week is available for editing in the test.
            timetableManagerPage.AddTimetableWeekSQL();

            // 6) Click on the Manage Periods and Days button
            await timetableManagerPage.ClickManagePeriodsAndDaysButton();

            // 7) Click on the Timetable Week in the tree
            await timetableManagerPage.GetCreatedTimetableWeek();

            // 8) Perform the workflow involving the popup
            TimetableManagerPopupFunctions editATimetableWeek = await timetableManagerPage.EditTimetableWeekAndGetPopupAsync();
            timetableManagerPage = await editATimetableWeek.EditTimetableWeekFieldsAndReturnToMainWindow();

            // 9) Assertion - check that the Timetable Week has been edited
            var editedTimetableWeek = await timetableManagerPage.GetEditedTimetableWeek();
            Assert.That(editedTimetableWeek, Is.Not.Null, $"Timetable week found");

            // 10) Test data clean up - delete the created Timetable Period to ensure the test is re-usable
            timetableManagerPage.DeleteEditedTimetableWeek();


        }




        [Test]
        [Category("AutoLogin")]
        [Category("Timetable Manager")]
        public async Task Test_Edit_Timetable_Day()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Timetable Manager" and click on the module.
            var timetableManagerPage = await homePage.NavigateToModuleAsync<TimetableManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await timetableManagerPage.NavigateToTabAsync("Configuration");

            // 5) Delete any test data left over from any potential failed previous runs of the test.
            timetableManagerPage.DeleteEditedTimetableDay();
            timetableManagerPage.DeleteTimetableWeek();
            // Then add a timetable week and day directly to the database to ensure a test week is available for editing in the current test.
            timetableManagerPage.AddTimetableWeekSQL();
            timetableManagerPage.AddTimetableDaySQL();

            // 6) Click on the Manage Periods and Days button
            await timetableManagerPage.ClickManagePeriodsAndDaysButton();

            // 7) Click on the Timetable Week and then Timetable Day in the tree
            await timetableManagerPage.GetCreatedTimetableWeek();
            await timetableManagerPage.GetSQLCreatedTimetableDay();

            // 8) Perform the workflow involving the popup
            TimetableManagerPopupFunctions editATimetableWeek = await timetableManagerPage.EditTimetableDayAndGetPopupAsync();
            timetableManagerPage = await editATimetableWeek.EditTimetablDayFieldsAndReturnToMainWindow();

            // 9) Assertion - check that the Timetable Week has been edited
            var editedTimetableDay = await timetableManagerPage.GetEditedTimetableDay();
            Assert.That(editedTimetableDay, Is.Not.Null, $"Timetable day found");

            // 10) Test data clean up - delete the created Timetable Day and Week to ensure the test is re-usable
            timetableManagerPage.DeleteEditedTimetableDay();
            timetableManagerPage.DeleteTimetableWeek();

        }

    }
}
