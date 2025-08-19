using Azure;
using Microsoft.Playwright;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerPopupUIMap;

namespace TAF_iSAMS.Pages.Modules.SchoolManager
{
    /// <summary>
    /// Page Object representing the "Create Form" popup window.
    /// Contains methods for interacting with the popup and returning to the parent page.
    /// Inherits ModuleBasePage assuming it might have a complex frame structure similar to main modules.
    /// Adjust inheritance to BasePage if the popup structure is simple (no complex iframes).
    /// </summary>
    class SchoolManagerPopupFunctions : ModuleBasePage
    {
        // Store a reference to the page object that opened this popup.
        private readonly SchoolManagerFunctions _parentPage;

        /// <summary>
        /// Private constructor to enforce use of the factory pattern.
        /// </summary>
        /// <param name="page">The IPage instance for this popup window.</param>
        /// <param name="parent">The page object instance of the parent window.</param>
        private SchoolManagerPopupFunctions(IPage page, SchoolManagerFunctions parent)
            : base(page) // Call the base constructor with the popup's page
        {
            _parentPage = parent ?? throw new ArgumentNullException(nameof(parent));
            TestContext.WriteLine("CreateFormPopupPageFunctions base constructor called.");
        }

        /// <summary>
        /// Asynchronous factory method to create and initialize an instance of CreateFormPopupPageFunctions.
        /// </summary>
        /// <param name="page">The IPage instance for the new popup window.</param>
        /// <param name="parent">The page object instance of the parent window that opened this popup.</param>
        /// <returns>A fully initialized CreateFormPopupPageFunctions instance.</returns>
        public static async Task<SchoolManagerPopupFunctions> CreateAsync(IPage page, SchoolManagerFunctions parent)
        {
            var instance = new SchoolManagerPopupFunctions(page, parent);
            TestContext.WriteLine("Initializing frames for CreateForm popup...");

            // Initialize frames for THIS popup using the adapted base method.
            // Pass null for moduleSelector to indicate popup context.
            // Set isPopup: true for explicit indication.
            await instance.InitializeFramesAsync(moduleSelector: null, contentFrameSelector: null, isPopup: true);

            // Add a wait here if needed for elements within the popup's frames to load
            // Example: await instance.ContentNavigator.WaitForSelectorAsync(Ui.FormNameInput);
            if (NavigationHandler.ContentNavigator == null && NavigationHandler.iFrame1 == null)
            {
                TestContext.WriteLine("Warning: ContentNavigator is null after InitializeFramesAsync in CreateFormPopupPageFunctions.CreateAsync. Popup might be simple or frame detection failed.");
                // If popup is simple (no frames), ContentNavigator might remain null.
                // There are some different frames used for popups, such as iFrame1 so we need to account for that.
                // Interaction methods might need to use Page directly or check ContentNavigator.
            }
            else
            {
                // Optional: Wait for a key element in the content frame to ensure it's ready
                // Better element needs to be chosen because FormNameInput/surnameFilter is not present in all popups.
                try
                {
                    if(NavigationHandler.ContentNavigator == null)
                        await instance.WaitForSelectorAsync(Ui.surnameFilterSelector, NavigatorTypes.Types.iFrame1, timeout: 5000); // Wait 5s for form name input
                    else
                        await instance.WaitForSelectorAsync(Ui.FormNameInput, NavigatorTypes.Types.Content, timeout: 5000); // Wait 5s for form name input
                }
                catch (TimeoutException)
                {
                    TestContext.WriteLine($"Warning: Key element '{Ui.FormNameInput}' not found in popup content frame after initialization.");
                }
            }


            TestContext.WriteLine("CreateFormPopupPageFunctions instance fully initialized.");
            return instance;
        }

        // --- Interaction Methods ---
        /// <summary>
        /// Enters the form name into the corresponding input field.
        /// </summary>
        /// <param name="name">The name of the form.</param>
        public async Task<SchoolManagerFunctions> EnterFormDataAndReturnMainWindow(string name = "")
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");

            if (name == "")
                name = Ui.DummyFormName;

            await UpdateTextElement(Ui.FormNameInput, name, navigatorType);
            await SelectDropDownElement(Ui.FormRoomDropDown, Ui.FormRoomDropDownValue, navigatorType);
            await SelectDropDownElement(Ui.FormYearDropDown, Ui.FormYearDropDownValue, navigatorType);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EnterAcademicHouseDetailsAndReturnToMainWindow()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;
            // Use ContentNavigator if available and popup has frames, otherwise use Page directly
            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");

            await UpdateTextElement(Ui.HouseNameSelector, Ui.HouseName, navigatorType);
            await UpdateTextElement(Ui.HouseCodeSelector, Ui.HouseCode, navigatorType);
            await SelectDropDownElement(Ui.HouseMasterDropDown, Ui.HouseMasterDropDownValue, navigatorType);
            await ClickElement(Ui.HouseGenderMale, navigatorType);
            await ClickElement(Ui.HouseTypeAcademic, navigatorType);
            return await SaveAndReturnToMainWindow();
        }


        public async Task<SchoolManagerFunctions> SelectTutorAndReturnToMainWindow()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");


            await SelectDropDownElement(Ui.TutorDropDown, Ui.FormTutorDropDownValue, navigatorType);
            return await SaveAndReturnToMainWindow();
        }
        public async Task<SchoolManagerFunctions> SelectAlternativeTutorAndReturnToMainWindow()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");


            await SelectDropDownElement(Ui.TutorDropDown, Ui.AlternateFormTutorDropDownValue, navigatorType);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EnterYearDataAndReturnToMainWindow()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)            
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");
            

            await UpdateTextElement(Ui.YearNameSelector, Ui.YearName, navigatorType);
            await UpdateTextElement(Ui.YearCodeSelector, Ui.YearCode, navigatorType);
            await SelectDropDownElement(Ui.TutorDropDown, Ui.TutorDropDownOption, navigatorType);
            await SelectDropDownElement(Ui.NCYearDropDown, Ui.NCYearDropDownOption, navigatorType);
            await SelectDropDownElement(Ui.DfEYearGroupDropDown, Ui.DfEYearGroupDropDownOption, navigatorType);
            await SelectDropDownElement(Ui.YearGroupDropDown, Ui.YearGroupDropDownOption, navigatorType);
            await SelectDropDownElement(Ui.AverageStartingAgeDropDown, Ui.AverageStartingAgeDropDownOption, navigatorType);

            return await SaveAndReturnToMainWindow();
        }


        public async Task<SchoolManagerFunctions> EnterYearBlockDataAndReturnToMainWindow()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)            
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");
            

            await UpdateTextElement(Ui.YearBlockNameSelector, Ui.YearBlockName, navigatorType);
            await UpdateTextElement(Ui.YearBlockCodeSelector, Ui.YearBlockCode, navigatorType);
            await SelectDropDownElement(Ui.YearBlockYearDropDown, Ui.YearBlockYearDropDownValue, navigatorType);

            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EnterTermDataAndReturnToMainWindow()
        {
            await SelectDropDownElement(Ui.TermNameDropDown, Ui.TermNameDropDownValue, NavigatorTypes.Types.Page);
            await UpdateTextElement(Ui.SchoolYearSelector, Ui.SchoolYear, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EnterTeachingDepartmentAndReturnToMainWindow()
        {
            await UpdateTextElement(Ui.DepartmentNameSelector, Ui.DepartmentName, NavigatorTypes.Types.Page);
            await UpdateTextElement(Ui.DepartmentCodeSelector, Ui.DepartmentCode, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EnterDivisionDataAndReturnToMainWindow()
        {
            await UpdateTextElement(Ui.DivisionNameSelector, Ui.DivisionName, NavigatorTypes.Types.Page);
            await UpdateTextElement(Ui.DivisionCodeSelector, Ui.DivisionCode, NavigatorTypes.Types.Page);
            await SelectDropDownElement(Ui.DivisionYearGroupDropDown, Ui.DivisionYearGroupDropDownValue, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditUpdateSchoolForm()
        {
            await SelectDropDownElement(Ui.FormRoomDropDown, Ui.FormRoomDropDownValue, NavigatorTypes.Types.Content);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditUpdateAcademicHouse()
        {
            await SelectDropDownElement(Ui.HouseMasterDropDown, Ui.HouseMasterDropDownValue, NavigatorTypes.Types.Content);
            return await SaveAndReturnToMainWindow();
        }        
        public async Task<SchoolManagerFunctions> EditUpdateTempDepartment()
        {
            await UpdateTextElement(Ui.DepartmentCodeSelector, Ui.DepartmentCode, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditUpdateTempDivision()
        {
            await SelectDropDownElement(Ui.DivisionYearGroupDropDown, Ui.DivisionYearGroupDropDownValue, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditUpdateTerm()
        {
            await UpdateTextElement(Ui.SchoolYearSelector, Ui.SchoolYear, NavigatorTypes.Types.Page);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditCreatedYearBlock()
        {
            await UpdateTextElement(Ui.YearBlockNameSelector, Ui.UpdatedYearBlockName, NavigatorTypes.Types.Content);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> EditCreatedYear()
        {
            await UpdateTextElement(Ui.YearNameSelector, Ui.UpdatedYearName, NavigatorTypes.Types.Content);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<SchoolManagerFunctions> AddPupilsToForm()
        {
            await ReInitialiseFrames();

            FoundElementInfo ele = await FindElementOnPage("selectall"); // Debugging: Find Element in the dom through js
            //Click all pupils
            await ClickElement(Ui.FormsTickAllPupils, NavigatorTypes.Types.iFrame1); //Playwright fails to find any selector so this uses js fallback
            //Click Save And close
            await ClickElement(Ui.FormAddPupilsSaveButton, NavigatorTypes.Types.iFrame1); //Playwright fails to find any selector so this uses js fallback

            //This is cancelling the js dialogue that appears when clicking 'Save & Close'

            //Page.Dialog += async (_, dialog) =>
            //{
            //    TestContext.WriteLine($"📣 Dialog detected: {dialog.Message}");
            //    await dialog.AcceptAsync(); // Simulates clicking "OK"
            //};

            //Click ok on popup
            return await SaveAndReturnToMainWindow();
        }
        // Add other interaction methods using ContentNavigator (or Page directly) and CreateFormPopupPageUiMap selectors...
        // Example:
        // public async Task SelectFormTutorAsync(string tutorName) { ... }

        // --- Methods to Close Popup and Return Parent ---


        public async Task<SchoolManagerFunctions> SaveAndReturnToMainWindow(string? locator = null)
        {
            SchoolManagerFunctions schoolManagerPage = null;
            TestContext.WriteLine("Acting: Saving popup and returning to parent.");
            try
            {
                schoolManagerPage = await SaveAndReturnToParentAsync(locator ?? Ui.SaveButton);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed during SaveAndReturnToParentAsync: {ex.Message}\n{ex.StackTrace}");
            }
            TestContext.WriteLine("Returned to parent page object.");
            return schoolManagerPage;
        }


        public async Task<SchoolManagerFunctions> SaveAndReturnToParentAsync(string locator) =>
         await ClickElementCloseAndWaitForParentAsync(locator);

        /// <summary>
        /// Clicks the 'Save' button, waits for the popup to close, and returns the parent page object.
        /// </summary>
        /// <returns>The parent SchoolManagerFunctions page object instance.</returns>
        public async Task<SchoolManagerFunctions> CloseWindowAndReturnToParentAsync(string locator) =>
         await ClickElementCloseAndWaitForParentAsync(locator);


        /// <summary>
        /// Clicks the 'Close' button, waits for the popup to close, and returns the parent page object.
        /// </summary>
        /// <returns>The parent SchoolManagerFunctions page object instance.</returns>
        public async Task<SchoolManagerFunctions> CloseWindowAndReturnToParentAsync()
        {
            return await ClickElementCloseAndWaitForParentAsync(Ui.CloseButton);
        }

        /// <summary>
        /// Private helper to click an element (assumed to be on the main popup page),
        /// wait for the Page.Close event, and return the parent page object.
        /// </summary>
        /// <param name="locator">The selector for the element to click (which should close the popup).</param>
        /// <param name="timeoutMilliseconds">Timeout for waiting for the close event.</param>
        /// <returns>The parent SchoolManagerFunctions page object instance.</returns>
        private async Task<SchoolManagerFunctions> ClickElementCloseAndWaitForParentAsync(string locator, int timeoutMilliseconds = DefaultPopupTimeout)
        {
            // Check if the locator is valid
            if (string.IsNullOrEmpty(locator)) throw new ArgumentException("Locator cannot be null or empty.", nameof(locator));

            TestContext.WriteLine($"Attempting to click '{locator}' directly on page, wait for close event (Timeout: {timeoutMilliseconds}ms), and return parent.");

            // Setup TaskCompletionSource to signal when the page close event is received
            var closeDetected = new TaskCompletionSource<bool>();
            // Setup CancellationTokenSource for the timeout
            var timeoutCancel = new CancellationTokenSource(timeoutMilliseconds);
            timeoutCancel.Token.Register(() => closeDetected.TrySetCanceled()); // Cancel TCS on timeout

            // Define the event handler using a lambda expression
            EventHandler<IPage> handler = null; // Declare handler variable
            handler = (sender, pageArgs) =>
            {
                // Check if the closed page is the one we are interacting with
                if (pageArgs == this.Page)
                {
                    TestContext.WriteLine($"Close event received for page: {pageArgs.Url}");
                    closeDetected.TrySetResult(true); // Signal that the page closed
                    Page.Close -= handler; // Unsubscribe the handler
                }
            };

            // Subscribe to the Close event *before* clicking the element
            Page.Close += handler;

            try
            {
                TestContext.WriteLine($"Clicking element '{locator}' directly via Page.");
                await Page.Locator(locator).ClickAsync();
                TestContext.WriteLine($"Clicked element '{locator}'. Waiting for Page.Close event...");

                // Wait for the close event to be signaled or timeout
                await closeDetected.Task;
                TestContext.WriteLine("Page closed event detected successfully after click.");
            }
            catch (OperationCanceledException) // Catches cancellation from CancellationTokenSource
            {
                TestContext.WriteLine($"Warning: Timeout ({timeoutMilliseconds}ms) waiting for Page.Close event after clicking '{locator}'.");
                Page.Close -= handler; // Ensure unsubscribe on timeout
                // Depending on requirements, you might want to throw here or just return the parent anyway
                // throw new PlaywrightException($"Timeout waiting for page to close after clicking '{locator}'.");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error during click/close operation for locator '{locator}': {ex.Message}");
                Page.Close -= handler; // Ensure unsubscribe on any other error
                throw; // Re-throw the exception
            }
            finally
            {
                // Dispose the CancellationTokenSource
                timeoutCancel.Dispose();
            }

            // Return the stored reference to the parent page object
            return _parentPage;
        }
    }
}
