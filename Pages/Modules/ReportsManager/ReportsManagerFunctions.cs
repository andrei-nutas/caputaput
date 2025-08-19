using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.ReportsManager.ReportsManagerUIMap;

namespace TAF_iSAMS.Pages.Modules.ReportsManager
{
    internal class ReportsManagerFunctions : ModuleBasePage
    {

        private ReportsManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the School Manager page.
        /// </summary>
        /// 
        #region Setup
        public static async Task<ReportsManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating ReportsManagerFunctions instance.");
            var instance = new ReportsManagerFunctions(page);
            // Initialize the iframe hierarchy for the Reports Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator, Ui.ModuleContentFrame);
            Console.WriteLine($"ReportsManagerFunctions instance created and initialized.");
            return instance;
        }
        #endregion


    }
}
