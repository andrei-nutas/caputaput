using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.StudentManager.StudentManagerUIMap;
using Sql = TAF_iSAMS.Pages.Modules.StudentManager.StudentManagerSQL;



namespace TAF_iSAMS.Pages.Modules.StudentManager
{
    class StudentManagerFunctions : ModuleBasePage
    {
        SQLConnector connector = new SQLConnector();


        private StudentManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the Student Manager page.
        /// </summary>
        public static async Task<StudentManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating StudentManagerFunctions instance.");
            var instance = new StudentManagerFunctions(page);
            // Initialize the iframe hierarchy for the Student Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator);
            Console.WriteLine($"StudentManagerFunctions instance created and initialized.");
            return instance;
        }

        public async Task EnterPupilDetails()
        {
            await SelectPupilTitle();
            await AddPupilForename();
            await AddPupilSurname();
            await AddPupilDateOfBirth();
        }

        public async Task SelectPupilTitle() =>
            await SelectDropDownElement(Ui.TitleDropdownField, Ui.TitleDropdownValue, NavigatorTypes.Types.DataFrame);

        public async Task AddPupilForename() =>
            await UpdateTextElement(Ui.ForenameFieldSelector, Ui.ForenameFieldValue, NavigatorTypes.Types.DataFrame);

        public async Task AddPupilSurname() =>
            await UpdateTextElement(Ui.SurnameFieldSelector, Ui.SurnameFieldValue, NavigatorTypes.Types.DataFrame);

        public async Task AddPupilDateOfBirth() =>
            await UpdateTextElement(Ui.DateOfBirthField, Ui.DateOfBirthValue, NavigatorTypes.Types.DataFrame);

        public async Task ExecuteNextButton() =>
            await ClickElement(Ui.ExecuteNextButton, NavigatorTypes.Types.DataFrame);

        public async Task<bool> Stage1CompletedVisible()
        {
            return await WaitForElementToBeVisible(Ui.Stage1CompletedMessage, NavigatorTypes.Types.txtFrameOptions);
        }

        public void DeleteTestPupil() =>
            connector.ExecuteQuery(Sql.SqlDeleteTestStudent);


        public void AddCurrentTestPupil() =>
            connector.ExecuteQuery(Sql.SqlAddSearchCurrentPupil);

        public void DeleteSearchCurrentPupil() =>
            connector.ExecuteQuery(Sql.SqlDeleteSearchCurrentPupil);


        public async Task EnterCurrentStudentSearchDetails()
        {
            await AddForenameSearchValue();
            await AddSurnameSearchValue();
            await ExecuteSearchButton();
        }

        public async Task EnterApplicantStudentSearchDetails()
        {
            await AddApplicantForenameSearchValue();
            await AddApplicantSurnameSearchValue();
            await ExecuteSearchButton();
        }


        public async Task AddForenameSearchValue() =>
            await UpdateTextElement(Ui.StudentForenameSearchSelector, Ui.StudentForenameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task AddSurnameSearchValue() =>
            await UpdateTextElement(Ui.StudentSurnameSearchSelector, Ui.StudentSurnameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task AddApplicantForenameSearchValue() =>
            await UpdateTextElement(Ui.ApplicantForenameSearchSelector, Ui.ApplicantForenameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task AddApplicantSurnameSearchValue() =>
            await UpdateTextElement(Ui.ApplicantSurnameSearchSelector, Ui.ApplicantSurnameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task ExecuteSearchButton() =>
            await ClickElement(Ui.ExecuteSearchButton, NavigatorTypes.Types.DataFrame);


        public async Task ClickPupilNameOnPage()
        {
            await ClickRowElement(Ui.PupilFullNameValueSearchResult, Ui.PupilFullNameValueSearchResult, NavigatorTypes.Types.DataFrame);
        }

        public async Task ClickApplicantNameOnPage()
        {
            await ClickRowElement(Ui.ApplicantFullNameValueSearchResult, Ui.ApplicantFullNameValueSearchResult, NavigatorTypes.Types.DataFrame);
        }

        public async Task<bool> PupilEnrolmentTabVisible()
        {
            return await WaitForElementToBeVisible(Ui.ViewPupilEnrolmentButton, NavigatorTypes.Types.DataFrame);
        }

        public async Task<bool> FamilyTabVisible()
        {
            return await WaitForElementToBeVisible(Ui.ViewApplicantFamilyButton, NavigatorTypes.Types.DataFrame);
        }

        public void AddSearchApplicant() =>
            connector.ExecuteQuery(Sql.SqlAddSearchApplicant);

        public void DeleteSearchApplicant() =>
            connector.ExecuteQuery(Sql.SqlDeleteSearchApplicant);


        public async Task EnterFormerPupilSearchDetails()
        {
            await AddFormerPupilForenameSearchValue();
            await AddFormerPupilSurnameSearchValue();
            await ExecuteSearchButton();
        }

        public async Task AddFormerPupilForenameSearchValue() =>
            await UpdateTextElement(Ui.FormerPupilForenameSearchSelector, Ui.FormerPupilForenameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task AddFormerPupilSurnameSearchValue() =>
            await UpdateTextElement(Ui.FormerPupilSurnameSearchSelector, Ui.FormerPupilSurnameSearchValue, NavigatorTypes.Types.DataFrame);

        public async Task ClickFormerPupilNameOnPage()
        {
            await ClickRowElement(Ui.FormerPupilFullNameValueSearchResult, Ui.FormerPupilFullNameValueSearchResult, NavigatorTypes.Types.DataFrame);
        }

        public async Task<bool> FormerPupilFamilyTabVisible()
        {
            return await WaitForElementToBeVisible(Ui.ViewFormerPupilFamilyButton, NavigatorTypes.Types.DataFrame);
        }

        public void AddFormerPupil() =>
            connector.ExecuteQuery(Sql.SqlAddSearchFormerPupil);

        public void DeleteFormerPupil() =>
            connector.ExecuteQuery(Sql.SqlDeleteSearchFormerPupil);

        public async Task UpdateStudentMiddleNameField()
        {
            await ReInitialiseFrames();// now automatically if needed
            await UpdateTextElement(Ui.StudentMiddleNameSelector, Ui.StudentMiddleNameValue, NavigatorTypes.Types.Record);
        }

        public async Task ClickUpdateDataButton() =>
            await ClickElement(Ui.UpdateDataButton, NavigatorTypes.Types.Record);

        public async Task<bool> PupilUpdatedMessageVisible() =>
            await WaitForElementToBeVisible(Ui.UpdatePupilMemberMessage, NavigatorTypes.Types.DataFrame);

        public async Task TickAllSearchResults() =>
            await ClickElement(Ui.SelectAllSearchResults, NavigatorTypes.Types.DataFrame);

    }
}
