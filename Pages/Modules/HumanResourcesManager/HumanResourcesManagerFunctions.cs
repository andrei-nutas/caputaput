using Microsoft.Playwright;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.HumanResourcesManager.HumanResourcesManagerUIMap;
using Sql = TAF_iSAMS.Pages.Modules.HumanResourcesManager.HumanResourcesManagerSQL;


namespace TAF_iSAMS.Pages.Modules.HumanResourcesManager
{
    public class HumanResourcesManagerFunctions : ModuleBasePage
    {
        // Private constructor to enforce use of the factory method.
        private HumanResourcesManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the Human Resources Manager page.
        /// </summary>
        /// 

        SQLConnector connector = new SQLConnector();

        public static async Task<HumanResourcesManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating HumanResourcesManagerFunctions instance.");
            var instance = new HumanResourcesManagerFunctions(page);
            // Initialize the iframe hierarchy for the Human Resources Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator);
            Console.WriteLine($"HumanResourcesManagerFunctions instance created and initialized.");
            return instance;
        }


        public async Task EnterStaffDetails()
        {
            await AddSchoolInitials();
            await SelectStaffTitle();
            await AddForename();
            await AddSurname();
            await SelectStatusType();
        }

        public async Task EnterStaffSearchDetails()
        {
            await AddForenameSearch();
            await AddSurnameSearch();
        }


        public async Task AddSchoolInitials() =>
            await UpdateTextElement(Ui.SchoolInitialsSelector, Ui.SchoolInitialsValue, NavigatorTypes.Types.txtFrameLeft);

        public async Task AddForename() =>
            await UpdateTextElement(Ui.ForenameFieldSelector, Ui.ForenameFieldValue, NavigatorTypes.Types.txtFrameLeft);

        public async Task AddSurname() =>
            await UpdateTextElement(Ui.SurnameFieldSelector, Ui.SurnameFieldValue, NavigatorTypes.Types.txtFrameLeft);

        public async Task SelectStaffTitle() =>
            await SelectDropDownElement(Ui.TitleDropdownField, Ui.TitleDropdownValue, NavigatorTypes.Types.txtFrameLeft);

        public async Task SelectStatusType() =>
            await SelectDropDownElement(Ui.StatusDropdownField, Ui.StatusDropdownValue, NavigatorTypes.Types.txtFrameLeft);

        public async Task ExecuteNextStepButton() =>
            await ClickElement(Ui.ExecuteNextStepButton, NavigatorTypes.Types.txtFrameLeft);

        public async Task<bool> NewStaffCreatedMessageVisible() =>
            await WaitForElementToBeVisible(Ui.NewStaffCreatedSuccessMessage, NavigatorTypes.Types.txtFrameLeft);

        public void DeleteStaffMember() =>
            connector.ExecuteQuery(Sql.SqlDeleteStaffMember);

        public void AddStaffSearchMember() =>
            connector.ExecuteQuery(Sql.SqlAddSearchStaffMember);

        public void DeleteStaffSearchMember() =>
            connector.ExecuteQuery(Sql.SqlDeleteSearchStaffMember);

        public async Task AddForenameSearch() =>
            await UpdateTextElement(Ui.StaffForenameSearchSelector, Ui.StaffForenameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task AddSurnameSearch() =>
            await UpdateTextElement(Ui.StaffSurnameSearchSelector, Ui.StaffSurnameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task ExecuteSearchButton() =>
            await ClickElement(Ui.ExecuteSearchButton, NavigatorTypes.Types.DataFrame);

        public async Task TickAllSearchResults() =>
            await ClickElement(Ui.SelectAllSearchResults, NavigatorTypes.Types.DataFrame);

        public async Task ClickStaffNameOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            await ClickRowElement(Ui.StaffFullNameValueSearchResult, Ui.StaffFullNameValueSearchResult, NavigatorTypes.Types.DataFrame);
        }

        public async Task<bool> StaffEnrolmentTabVisible()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await WaitForElementToBeVisible(Ui.ViewStaffEnrolmentButton, NavigatorTypes.Types.DataFrame);
        }
        
        public async Task UpdateStaffSchoolInitialsField()
        {
            await ReInitialiseFrames();// now automatically if needed
            await UpdateTextElement(Ui.StaffSchoolInitialsSelector, Ui.StaffSchoolInitialsValue, NavigatorTypes.Types.Record);
        }

        public async Task ClickUpdateDataButton() =>
        await ClickElement(Ui.UpdateDataButton, NavigatorTypes.Types.Record);

        public async Task<bool> StaffUpdatedMessageVisible() =>
        await WaitForElementToBeVisible(Ui.UpdateStaffMemberMessage, NavigatorTypes.Types.DataFrame);

        public void DeleteEditedStaffMember() =>
        connector.ExecuteQuery(Sql.SqlDeleteEditedStaffMember);

        public void AddSecondStaffMember() =>
            connector.ExecuteQuery(Sql.SqlAddSecondSearchStaffMember);

        public async Task<HumanResourcesManagerPopupFunctions> CreateGroupEditPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            HumanResourcesManagerPopupFunctions? createGroupEditPopup = null;
            try
            {
                return createGroupEditPopup = await ClickGroupEditAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }


        public async Task<HumanResourcesManagerPopupFunctions> ClickGroupEditAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Year button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking group edit drop down button using selector: {Ui.GroupEditDropDownSelector}");
                // Use ContentNavigator to interact within the correct frame
                await SelectDropDownElement(Ui.GroupEditDropDownSelector, Ui.GroupEditDropDownSelectorOption, NavigatorTypes.Types.txtFrameTop);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<HumanResourcesManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await HumanResourcesManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateYearPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<HumanResourcesManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }

    }
}
