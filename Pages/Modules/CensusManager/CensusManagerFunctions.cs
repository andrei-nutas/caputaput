using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Modules.AdmissionsManager;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.CensusManager.CensusManagerUIMap;

namespace TAF_iSAMS.Pages.Modules.CensusManager
{
    class CensusManagerFunctions : ModuleBasePage
    {

        // Private constructor to enforce use of the factory method.
        private CensusManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the Census Manager page.
        /// </summary>
        /// 

        public static async Task<CensusManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating CensusManagerFunctions instance.");
            var instance = new CensusManagerFunctions(page);
            // Initialize the iframe hierarchy for the Census Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator);
            Console.WriteLine($"CensusManagerFunctions instance created and initialized.");
            return instance;
        }

        public SQLConnector connector = new SQLConnector();

        #region Configuration > Manage Census Settings
        public async Task SchoolTypeNonIndependent()=>
            await SelectDropDownElement(Ui.SchoolTypeDropDown, Ui.SchoolTypeNonIndependent, NavigatorTypes.Types.NestedContent);
        public async Task SchoolTypeIndependent() =>
           await SelectDropDownElement(Ui.SchoolTypeDropDown, Ui.SchoolTypeIndependent, NavigatorTypes.Types.NestedContent);
        #endregion

        #region Build Tab
        public async Task StartISCCensus() =>
            await ClickRowElement(Ui.StartISCCensusRowText, Ui.StartCensusButton, NavigatorTypes.Types.NestedContent);
        public async Task StartSchoolLevelAnnualCensus() =>
            await ClickRowElement(Ui.StartSchoolLevelAnnualCensusRowText, Ui.StartCensusButton, NavigatorTypes.Types.NestedContent);
        public async Task StartSchoolCensus() =>
            await ClickRowElement(Ui.StartSchoolCensusRowText, Ui.StartCensusButton, NavigatorTypes.Types.NestedContent);
        public async Task StartSchoolWorkforceCensus() =>
            await ClickRowElement(Ui.StartWorkforceCensusRowText, Ui.StartCensusButton, NavigatorTypes.Types.NestedContent);
        #endregion

        #region Census
        public async Task ValidateCensus() =>
            await ClickElement(Ui.ValidateCensusButton, NavigatorTypes.Types.NestedContent);

        public async Task<IElementHandle> GetViewReportButton() =>
            await GetElement(Ui.ViewAndDownloadCensusReportButton, NavigatorTypes.Types.NestedContent);

        public async Task<bool> ViewReportButtonVisible()=>       
            await IsElementVisible(Ui.ViewAndDownloadCensusReportButton, NavigatorTypes.Types.NestedContent);
        public async Task<bool> ViewSummaryButtonVisible() =>
            await IsElementVisible(Ui.ViewSummaryReportAndDownloadCensusButton, NavigatorTypes.Types.NestedContent);
        public async Task<bool> WaitForBoardingHouseToBeVisible() =>
            await WaitForElementToBeVisible(Ui.BoardingHouseIndicator, NavigatorTypes.Types.NestedContent);
        #endregion

        #region SQL Commands
        public void SQLSetSchoolTypeIndependent()=>       
            connector.ExecuteQuery(Ui.SQLSchoolTypeIndependent);
        public void SQLSetSchoolTypeNonIndependent() =>
           connector.ExecuteQuery(Ui.SQLSchoolTypeNonIndependent);
        #endregion

    }
}
