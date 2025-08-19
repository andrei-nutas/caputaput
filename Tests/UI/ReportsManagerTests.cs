using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Pages.Modules.ReportsManager;

namespace TAF_iSAMS.Tests.UI
{
    [TestFixture]
    // Optionally you can run tests within this fixture in parallel:
    [Parallelizable(ParallelScope.Children)]
    internal class ReportsManagerTests : BaseTest
    {
        [Test]
        [Category("AutoLogin")]
        [Category("ReportsManager")]
        public async Task Test_Navigate_To_Configuration_Tab()
        {
            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Admissions" and click on the module.
            var reportsManagerPage = await homePage.NavigateToModuleAsync<ReportsManagerFunctions>();

            // 4) In the top iFrame, click on "Configuration"
            //    We'll use your ModuleNavigationBar class to do that:
            await reportsManagerPage.NavigateToTabAsync("Configuration");

           // await reportsManagerPage.NavigateToOptionsAsync("Online Assessment System");

           // await reportsManagerPage.ClickContentElement("id='saveAsColour_blue'");
        }

    }
}
