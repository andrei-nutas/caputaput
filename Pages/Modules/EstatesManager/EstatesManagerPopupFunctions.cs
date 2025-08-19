using Microsoft.Playwright;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Modules.SchoolManager;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerPopupUIMap;

namespace TAF_iSAMS.Pages.Modules.EstatesManager
{
    /// <summary>
    /// Page Object representing the "Create Form" popup window.
    /// Contains methods for interacting with the popup and returning to the parent page.
    /// Inherits ModuleBasePage assuming it might have a complex frame structure similar to main modules.
    /// Adjust inheritance to BasePage if the popup structure is simple (no complex iframes).
    /// </summary>
    class EstatesManagerPopupFunctions : ModuleBasePage
    {
        // Store a reference to the page object that opened this popup.
        private readonly EstatesManagerFunctions _parentPage;

        /// <summary>
        /// Private constructor to enforce use of the factory pattern.
        /// </summary>
        /// <param name="page">The IPage instance for this popup window.</param>
        /// <param name="parent">The page object instance of the parent window.</param>
        private EstatesManagerPopupFunctions(IPage page, EstatesManagerFunctions parent)
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
        public static async Task<EstatesManagerPopupFunctions> CreateAsync(IPage page, EstatesManagerFunctions parent)
        {
            var instance = new EstatesManagerPopupFunctions(page, parent);
            TestContext.WriteLine("Initializing frames for CreateForm popup...");

            // Initialize frames for THIS popup using the adapted base method.
            // Pass null for moduleSelector to indicate popup context.
            // Set isPopup: true for explicit indication.
            await instance.InitializeFramesAsync(moduleSelector: null, contentFrameSelector: null, isPopup: true);

            // Add a wait here if needed for elements within the popup's frames to load
            // Example: await instance.ContentNavigator.WaitForSelectorAsync(Ui.FormNameInput);
            if (NavigationHandler.ContentNavigator == null)
            {
                TestContext.WriteLine("Warning: ContentNavigator is null after InitializeFramesAsync in CreateFormPopupPageFunctions.CreateAsync. Popup might be simple or frame detection failed.");
                // If popup is simple (no frames), ContentNavigator might remain null.
                // Interaction methods might need to use Page directly or check ContentNavigator.
            }
            else
            {
                // Optional: Wait for a key element in the content frame to ensure it's ready
                try
                {
                    await instance.WaitForSelectorAsync(Ui.BuildingNameLocator, NavigatorTypes.Types.Content, timeout: 5000); // Wait 5s for form name input
                }
                catch (TimeoutException)
                {
                    TestContext.WriteLine($"Warning: Key element '{Ui.BuildingNameLocator}' not found in popup content frame after initialization.");
                }
            }


            TestContext.WriteLine("CreateFormPopupPageFunctions instance fully initialized.");
            return instance;
        }

        // --- Interaction Methods ---
        public async Task<EstatesManagerFunctions> EnterSchoolBuildingDetails()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");

            await UpdateTextElement(Ui.BuildingNameLocator, Ui.NewBuildingName, navigatorType);
            await UpdateTextElement(Ui.BuildingInitialsLocator, Ui.NewBuildingInitials, navigatorType);
            return await SaveAndReturnToMainWindow();
        }

        public async Task<EstatesManagerFunctions> EnterSchoolClassroomDetails()
        {
            var navigatorType = NavigationHandler.ContentNavigator != null ? NavigatorTypes.Types.Content : NavigatorTypes.Types.Page;

            if (navigatorType == NavigatorTypes.Types.Page)
                TestContext.WriteLine("ContentNavigator is null, attempting to interact directly with Page.");

            await UpdateTextElement(Ui.ClassroomNameLocator, Ui.tempClassroomName, navigatorType);
            await UpdateTextElement(Ui.ClassroomInitialsLocator, Ui.tempClassroomInitials, navigatorType);
            await UpdateTextElement(Ui.ClassroomCapacityLocator, Ui.tempClassroomCapacity, navigatorType);
            await SelectDropDownElement(Ui.RoomTypesDropDown, Ui.RoomTypesDropDownOption, navigatorType);
            await SelectDropDownElement(Ui.ClassroomBuildingDropDown, Ui.ClassroomBuildingDropDownOption, navigatorType);
            return await SaveAndReturnToMainWindow();
        }

        // Add other interaction methods using ContentNavigator (or Page directly) and CreateFormPopupPageUiMap selectors...
        // Example:
        // public async Task SelectFormTutorAsync(string tutorName) { ... }

        // --- Methods to Close Popup and Return Parent ---

        public async Task<EstatesManagerFunctions> SaveAndReturnToMainWindow()
        {
            EstatesManagerFunctions schoolManagerPage = null;
            TestContext.WriteLine("Acting: Saving popup and returning to parent.");
            try
            {
                schoolManagerPage = await SaveAndReturnToParentAsync();

            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed during SaveAndReturnToParentAsync: {ex.Message}\n{ex.StackTrace}");
            }
            TestContext.WriteLine("Returned to parent page object.");
            return schoolManagerPage;
        }

        /// <summary>
        /// Clicks the 'Save' button, waits for the popup to close, and returns the parent page object.
        /// </summary>
        /// <returns>The parent SchoolManagerFunctions page object instance.</returns>
        public async Task<EstatesManagerFunctions> SaveAndReturnToParentAsync()
        {
            return await ClickElementCloseAndWaitForParentAsync(Ui.SaveButton);
        }

        /// <summary>
        /// Clicks the 'Close' button, waits for the popup to close, and returns the parent page object.
        /// </summary>
        /// <returns>The parent SchoolManagerFunctions page object instance.</returns>
        public async Task<EstatesManagerFunctions> CloseWindowAndReturnToParentAsync()
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
        private async Task<EstatesManagerFunctions> ClickElementCloseAndWaitForParentAsync(string locator, int timeoutMilliseconds = DefaultPopupTimeout)
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
