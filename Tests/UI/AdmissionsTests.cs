using TAF_iSAMS.Pages.Modules.AdmissionsManager;
using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    // Optionally you can run tests within this fixture in parallel:
    [Parallelizable(ParallelScope.Children)]
    public class AdmissionsTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("Admissions")]
        public async Task Test_Navigate_To_Configuration_Tab()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var admissionsPage = await homePage.NavigateToModuleAsync<AdmissionsManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await admissionsPage.NavigateToTabAsync("Configuration");

            // At this point, you should be in the Admissions -> Configuration area.

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Admissions")]
        public async Task Test_Create_A_New_Enquiry()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var admissionsPage = await homePage.NavigateToModuleAsync<AdmissionsManagerFunctions>();

            // 4) In the top iFrame, click on "Enquiry" tab
            await admissionsPage.NavigateToTabAsync("Enquiry");

            // 5) Input the mandatory data fields for a new applicant enquiry
            await admissionsPage.UpdateApplicantDetails();

            // 6) Click the Save Applicant button
            await admissionsPage.SaveApplicant();

            // 7) Assertion
            var auditVisible = await admissionsPage.ViewAuditVisible();
            Assert.That(auditVisible, Is.True, "Audit button found");

            // 8) Clean up data
            admissionsPage.RemoveApplicant();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Admissions")]
        public async Task Test_Search_For_Applicant()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var admissionsPage = await homePage.NavigateToModuleAsync<AdmissionsManagerFunctions>();

            // 4) Add a test applicant to search for - added via SQL directly into the database
            admissionsPage.AddSearchApplicant();

            // 5) In the top iFrame, click on "Applicants" tab
            await admissionsPage.NavigateToTabAsync("Applicants");

            // 6) Input the search data fields
            await admissionsPage.EnterPupilSearchData();

            // 7) Click "Execute Search" button
            await admissionsPage.ExecuteSearchButton();

            // 8) Click on the returned pupil
            await admissionsPage.ClickPupilNameOnPage();

            // 9) Assertion
            var auditVisible = await admissionsPage.ViewAuditVisible();
            Assert.That(auditVisible, Is.True, "Audit button found");

            // 10) Clean up test data - delete the test applicant
            admissionsPage.DeleteSearchApplicant();

        }


    }
}
