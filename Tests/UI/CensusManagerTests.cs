using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Pages.Modules.CensusManager;
using TAF_iSAMS.Pages.Modules.ReportsManager;


namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    // Optionally you can run tests within this fixture in parallel:
    [Parallelizable(ParallelScope.Children)]
    class CensusManagerTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("CensusManager")]
        public async Task Test_Navigate_To_Configuration_Tab()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var censusManagerPage = await homePage.NavigateToModuleAsync<CensusManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await censusManagerPage.NavigateTo("Configuration");


            await censusManagerPage.SchoolTypeNonIndependent();
                        
        }

        [Test]
        [Category("AutoLogin")]
        [Category("CensusManager")]
        public async Task Start_ISC_Census()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].
            
            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();
            // 3) In the search bar write "Admissions" and click on the module.
            var censusManagerPage = await homePage.NavigateToModuleAsync<CensusManagerFunctions>();
            // 4) Set the School Type to Independent as ISC census requires it.
            censusManagerPage.SQLSetSchoolTypeIndependent();
            // 5) Refresh the page by clicking the Build tab again.
            await censusManagerPage.NavigateTo("Build");
            // 6) Start ISC Census
            await censusManagerPage.StartISCCensus();
            // 7) The Validate Button needs some time to become clickable as we wait for the page to load the list of pupils. 
            bool boardingHouseVisible = await censusManagerPage.WaitForBoardingHouseToBeVisible();
            Assert.That(boardingHouseVisible, Is.True, "The Boarding House indicator should be visible on the screen.");

            // 8) Click the Validate Census Button
            await censusManagerPage.ValidateCensus();
            // 9) Assert that a button called 'View Report And Download Census' is visible on screen
            Assert.That(censusManagerPage.ViewReportButtonVisible, Is.True);
        }


        [Test]
        [Category("AutoLogin")]
        [Category("CensusManager")]
        public async Task Start_School_Census()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();
            // 3) In the search bar write "Admissions" and click on the module.
            var censusManagerPage = await homePage.NavigateToModuleAsync<CensusManagerFunctions>();
            // 4) Set the School Type to NonIndependent as School census requires it.
            censusManagerPage.SQLSetSchoolTypeNonIndependent();
            // 5) Refresh the page by clicking the Build tab again.
            await censusManagerPage.NavigateTo("Build");
            // 6) Start ISC Census
            await censusManagerPage.StartSchoolCensus();

            // 7) Click the Validate Census Button
            await censusManagerPage.ValidateCensus();
            // 8) Assert that a button called 'View Report And Download Census' is visible on screen
            Assert.That(censusManagerPage.ViewSummaryButtonVisible, Is.True);

            // 9) Teardown. Set School Type back to it's default
            censusManagerPage.SQLSetSchoolTypeIndependent();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("CensusManager")]
        public async Task Start_School_Workforce_Census()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();
            // 3) In the search bar write "Admissions" and click on the module.
            var censusManagerPage = await homePage.NavigateToModuleAsync<CensusManagerFunctions>();
            // 4) Set the School Type to NonIndependent as School Workforce census requires it.
            censusManagerPage.SQLSetSchoolTypeNonIndependent();
            // 5) Refresh the page by clicking the Build tab again.
            await censusManagerPage.NavigateTo("Build");
            // 6) Start ISC Census
            await censusManagerPage.StartSchoolWorkforceCensus();
            // 7) Click the Validate Census Button
            await censusManagerPage.ValidateCensus();
            // 8) Assert that a button called 'View Report And Download Census' is visible on screen
            Assert.That(censusManagerPage.ViewSummaryButtonVisible, Is.True);
            // 9) Teardown. Set School Type back to it's default
            censusManagerPage.SQLSetSchoolTypeIndependent();

        }

    }
}
