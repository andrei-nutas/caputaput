using Microsoft.Playwright;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.AdmissionsManager.AdmissionsManagerUIMap;
using Sql = TAF_iSAMS.Pages.Modules.AdmissionsManager.AdmissionsManagerSQL;


namespace TAF_iSAMS.Pages.Modules.AdmissionsManager
{
    public class AdmissionsManagerFunctions : ModuleBasePage
    {
        // Private constructor to enforce use of the factory method.
        private AdmissionsManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the Admissions Manager page.
        /// </summary>
        /// 

        SQLConnector connector = new SQLConnector();

        public static async Task<AdmissionsManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating AdmissionsManagerFunctions instance.");
            var instance = new AdmissionsManagerFunctions(page);
            // Initialize the iframe hierarchy for the Admissions Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator);
            Console.WriteLine($"AdmissionsManagerFunctions instance created and initialized.");
            return instance;
        }

        public async Task UpdateApplicantDetails()
        {
            await UpdateApplicantForename();
            await UpdateApplicantSurname();
            await SelectGenderType();
            await SelectBoardingStatusType();
            await SelectAdmissionsStatusType();
        }
        public async Task UpdateApplicantForename() =>
               await UpdateTextElement(Ui.ApplicantForenameSelector, Ui.ApplicantForename, NavigatorTypes.Types.NestedContent);
        public async Task UpdateApplicantSurname() =>
            await UpdateTextElement(Ui.ApplicantSurnameSelector, Ui.ApplicantSurname, NavigatorTypes.Types.NestedContent);
        public async Task SelectGenderType() =>
            await SelectDropDownElement(Ui.GenderDropDown, Ui.GenderType, NavigatorTypes.Types.NestedContent);
        public async Task SelectBoardingStatusType() =>
            await SelectDropDownElement(Ui.BoardingStatusDropDown, Ui.BoardingStatusType, NavigatorTypes.Types.NestedContent);
        public async Task SelectAdmissionsStatusType() =>
            await SelectDropDownElement(Ui.AdmissionsStatusDropDown, Ui.AdmissionsStatusType, NavigatorTypes.Types.NestedContent);

        public async Task<bool> ViewAuditVisible() =>
           await WaitForElementToBeVisible(Ui.ViewAuditButton, NavigatorTypes.Types.NestedTopFrame);

        public async Task SaveApplicant()
        {
            await ClickElement(Ui.SaveApplicantButton, NavigatorTypes.Types.Content);
            //await ReInitialiseFrames(); now automatically if needed
            //This call will refresh the page and put us in the Applicant page so the iFrames need to be reinitialised.
        }

        public void RemoveApplicant() =>
            connector.ExecuteQuery(Sql.SqlDeleteApplicant);

        public void AddSearchApplicant() =>
            connector.ExecuteQuery(Sql.SqlAddSearchApplicant);

        public void DeleteSearchApplicant() =>
            connector.ExecuteQuery(Sql.SqlDeleteSearchApplicant);

        public async Task EnterPupilSearchData()
        {
            await EnterApplicantForenameSearch();
            await EnterApplicantSurnameSearch();
        }

        public async Task EnterApplicantForenameSearch() =>
            await UpdateTextElement(Ui.ApplicantForenameSearchSelector, Ui.ApplicantForenameSearchValue, NavigatorTypes.Types.AdmissionsDataFrame);

        public async Task EnterApplicantSurnameSearch() =>
            await UpdateTextElement(Ui.ApplicantSurnameSearchSelector, Ui.ApplicantSurnameSearchValue, NavigatorTypes.Types.AdmissionsDataFrame);

        public async Task ExecuteSearchButton() =>
            await ClickElement(Ui.ExecuteSearchButton, NavigatorTypes.Types.NestedTopFrame);

        public async Task ClickPupilNameOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            await ClickRowElement(Ui.ApplicantFullNameValue, Ui.ApplicantFullNameValue, NavigatorTypes.Types.AdmissionsResultsFrame);
        }

    }
}
