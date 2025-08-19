using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.HomePage.HomePageUiMap;

namespace TAF_iSAMS.Pages.HomePage
{
    public class HomePageFunctions : BasePage
    {
        public HomePageFunctions(IPage page) : base(page) { }

        public async Task ClickViewAllModulesAsync()
        {
            Console.WriteLine($"Clicking on View All Modules button.");
            await Page.Locator(Ui.ViewAllModulesButton).ClickAsync();
            Console.WriteLine($"Clicked on View All Modules button.");
        }

        public async Task SearchAndClickModuleAsync(string moduleName)
        {
            Console.WriteLine($"Searching for module: {moduleName}");

            // Fill the module search input.
            await Page.Locator(Ui.ModuleSearchInput).FillAsync(moduleName);
            Console.WriteLine($"Filled the module search with: {moduleName}");

            // Retrieve the selector using the helper function.
            string moduleSelector = $"a[id='{ModuleSelectorHelper.GetModuleNr(moduleName)}']";
            Console.WriteLine($"Retrieved selector: {moduleSelector} for module: {moduleName}");

            // Click on the module using the retrieved selector.
            await Page.Locator(moduleSelector).ClickAsync();
            Console.WriteLine($"Clicked on the module using selector: {moduleSelector}");
        }
    }
}
