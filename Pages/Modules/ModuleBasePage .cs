using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using System;
using System.Threading.Tasks;
using Helper = TAF_iSAMS.Pages.Utilities.HelperFunctions;
using NavigationHandler = TAF_iSAMS.Pages.Utilities.NavigationHandler;

namespace TAF_iSAMS.Pages.Modules
{
    public abstract class ModuleBasePage : BasePage
    {
        // Default timeout for waiting for new pages or close events (in milliseconds)
        protected const int DefaultPopupTimeout = 30000;

        protected FrameManager FrameManager { get; private set; }

        // Store original module initialization parameters for reinitialization
        private string _moduleSelector;
        private string _contentFrameSelector;
        private bool _isPopup;

        protected ModuleBasePage(IPage page) : base(page) { }

        /// <summary>
        /// Initializes the common iframe structure for a module or a popup.
        /// </summary>
        public async Task InitializeFramesAsync(string moduleSelector, string contentFrameSelector = null, bool isPopup = false)
        {
            // Store initialization parameters for later reinitialization
            _moduleSelector = moduleSelector;
            _contentFrameSelector = contentFrameSelector;
            _isPopup = isPopup;

            await PerformFrameInitialization();
            FrameStateManager.MarkAsInitialized(Page);
        }

        /// <summary>
        /// Internal method to perform the actual frame initialization
        /// </summary>
        private async Task PerformFrameInitialization()
        {
            // Assign FrameManager (respects private set)
            FrameManager = new FrameManager(Page);
            bool isPopupWindow = _isPopup || string.IsNullOrEmpty(_moduleSelector);

            if (!isPopupWindow)
            {
                TestContext.WriteLine($"Initializing frames using module selector: {_moduleSelector} for page {Page.Url}");
                try
                {
                    // Wait briefly for the main module container to likely exist
                    await Page.WaitForSelectorAsync(_moduleSelector, new PageWaitForSelectorOptions { Timeout = 5000 });
                }
                catch (TimeoutException)
                {
                    TestContext.WriteLine($"Warning: Module selector not found within timeout: {_moduleSelector}");
                }
            }
            else
            {
                TestContext.WriteLine($"Initializing frames directly for page: {Page.Url} (Popup Context)");
                // Wait for a known element or a short delay to allow popup rendering
                await Page.WaitForTimeoutAsync(1000); // Allow popup rendering time
            }

            // Detect frames using the current page context
            await FrameManager.DetectAndInitialiseFrames(Page);
            // Small delay might be needed after frame detection if content loads async within frames
            await Page.WaitForTimeoutAsync(200);
            TestContext.WriteLine($"Frame detection complete for {Page.Url}");

            // Initialize Navigators
            await SetupNavigators();

            // Additional check for popup context
            if (isPopupWindow && NavigationHandler.ContentNavigator == null)
            {
                TestContext.WriteLine($"Warning: ContentNavigator is null for popup context: {Page.Url}. Frame detection might have failed or the popup structure is simpler.");
            }
        }

        /// <summary>
        /// Executes a frame operation with automatic retry and frame re-initialization
        /// Now includes checking for popup parent reinitialization needs AND dynamic frame detection
        /// </summary>
        public async Task<T> ExecuteWithFrameRetry<T>(Func<Task<T>> operation, string operationName = "Frame Operation")
        {
            // Check if we need to reinitialize frames before attempting operation (popup parent check)
            if (FrameStateManager.NeedsReinitialization(Page))
            {
                TestContext.WriteLine($"Frame reinitialization needed for {operationName} after popup close, reinitializing...");
                await PerformFrameInitialization();
                FrameStateManager.MarkAsInitialized(Page);
            }

            try
            {
                return await operation();
            }
            catch (Exception ex) when (IsActualFrameDetachmentError(ex))
            {
                TestContext.WriteLine($"Actual frame detachment detected for {operationName}: {ex.Message}");
                TestContext.WriteLine("Performing single reinitialization and retry...");

                await PerformFrameInitialization();
                FrameStateManager.MarkAsInitialized(Page);

                // Single retry only for actual detachment
                return await operation();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Navigator is not initialized"))
            {
                TestContext.WriteLine($"Navigator not initialized for {operationName}: {ex.Message}");
                TestContext.WriteLine("Possible new dynamic frame appeared, reinitializing frames...");

                // Give a moment for any new frames/dialogs to fully load
                await Task.Delay(500);
                await PerformFrameInitialization();
                FrameStateManager.MarkAsInitialized(Page);

                // Single retry after reinitialization
                return await operation();
            }
        }

        /// <summary>
        /// Only detect ACTUAL frame detachment, not timeouts or missing elements
        /// ENHANCED: Also detect popup-related frame issues
        /// </summary>
        private static bool IsActualFrameDetachmentError(Exception ex)
        {
            var message = ex.Message.ToLowerInvariant();
            return message.Contains("frame has been detached") ||
                   message.Contains("execution context was destroyed") ||
                   message.Contains("target closed") ||
                   message.Contains("page has been closed") ||
                   message.Contains("target page, context or browser has been closed") ||
                   message.Contains("element handle has been disposed");
        }

        /// <summary>
        /// Executes a void frame operation with automatic retry
        /// </summary>
        public async Task ExecuteWithFrameRetry(Func<Task> operation, string operationName = "Frame Operation")
        {
            await ExecuteWithFrameRetry(async () =>
            {
                await operation();
                return true;
            }, operationName);
        }

        /// <summary>
        /// Re-initializes frames, useful after navigation within a module that might change frame structure or references.
        /// </summary>
        public async Task ReInitialiseFrames()
        {
            TestContext.WriteLine($"Re-initializing frames for page: {Page.Url}");

            if (FrameManager == null)
            {
                TestContext.WriteLine("Error: FrameManager is null during ReInitialiseFrames. Call InitializeFramesAsync first.");
                return;
            }

            await Task.Delay(2000); // Short delay before re-detecting
            await PerformFrameInitialization(); // Use the stored initialization parameters
            FrameStateManager.MarkAsInitialized(Page);
        }

        public async Task SetupNavigators()
        {
            // Pass reference to this ModuleBasePage instance for minimal retry functionality
            NavigationHandler.ContentNavigator = (FrameManager.ContentFrame != null) ?
                new ContentNavigation(FrameManager.ContentFrame, null, this) : null;
            NavigationHandler.NestedContentNavigator = (FrameManager.NestedContentFrame != null) ?
                new ContentNavigation(FrameManager.NestedContentFrame, null, this) : null;
            NavigationHandler.NestedTopFrameNavigator = (FrameManager.NestedTopFrame != null) ?
                new ContentNavigation(FrameManager.NestedTopFrame, null, this) : null;
            NavigationHandler.AdmissionsRecordNavigator = (FrameManager.RecordFrame != null) ?
                new ContentNavigation(FrameManager.RecordFrame, null, this) : null;
            NavigationHandler.AdmissionsNestedRecordNavigator = (FrameManager.NestedRecordFrame != null) ?
                new ContentNavigation(FrameManager.NestedRecordFrame, null, this) : null;
            NavigationHandler.DialogBodyNavigator = (FrameManager.DialogBody != null) ?
                new ContentNavigation(FrameManager.DialogBody, null, this) : null;
            NavigationHandler.AdmissionsFrameDataNavigator = (FrameManager.DataFrame != null) ?
                new ContentNavigation(FrameManager.DataFrame, null, this) : null;
            NavigationHandler.AdmissionsFrameResultsNavigator = (FrameManager.ResultsFrame != null) ?
                new ContentNavigation(FrameManager.ResultsFrame, null, this) : null;
            NavigationHandler.PageNavigator = (Page != null) ?
                new ContentNavigation(null, Page, this) : null;
            NavigationHandler.NavigatorTxtFrameLeft = (FrameManager.txtFrameLeft != null) ?
                new ContentNavigation(FrameManager.txtFrameLeft, null, this) : null;
            NavigationHandler.NavigatorTxtFrameLeft = (FrameManager.LeftFrame != null) ?
                new ContentNavigation(FrameManager.txtFrameLeft, null, this) : null;
            NavigationHandler.NavigatorTxtFrameData = (FrameManager.DataFrame != null) ?
                new ContentNavigation(FrameManager.DataFrame, null, this) : null;
            NavigationHandler.RecordFrame = (FrameManager.RecordFrame != null) ?
                new ContentNavigation(FrameManager.RecordFrame, null, this) : null;
            NavigationHandler.txtFrameOptions = (FrameManager.txtFrameOptions != null) ?
                new ContentNavigation(FrameManager.txtFrameOptions, null, this) : null;
            NavigationHandler.OptionsFrame = (FrameManager.OptionsFrame != null) ?
                new ContentNavigation(FrameManager.OptionsFrame, null, this) : null;
            NavigationHandler.RightFrame = (FrameManager.RightFrame != null) ?
                new ContentNavigation(FrameManager.RightFrame, null, this) : null;
            NavigationHandler.LeftFrame = (FrameManager.LeftFrame != null) ?
                new ContentNavigation(FrameManager.LeftFrame, null, this) : null;
            NavigationHandler.iFrame1 = (FrameManager.iFrame1 != null) ?
                new ContentNavigation(FrameManager.iFrame1, null, this) : null;

            TestContext.WriteLine($"Frames re-initialized. Navigators updated (Content: {NavigationHandler.ContentNavigator != null}, Nested: {NavigationHandler.NestedContentNavigator != null})");
        }

        public async Task RetreiveFrameDOMInfo(IFrame frame)
        {
           await FrameDebugger.GetDomElementsAsync(frame);
        }

        public async Task DebugAllFramesDomAsync()
        {
            await FrameDebugger.DebugAllFramesDomAsync(Page);
        }
        public async Task<FoundElementInfo> FindElementOnPage(string elementToFind)
        {
            return await FrameDebugger.FindElementRecursivelyAsync(Page,elementToFind);
        }
        /// <summary>
        /// Navigates to a tab within the module's top navigation frame.
        /// </summary>
        /// <param name="linkText">The visible text of the tab/link to click.</param>
        public async Task NavigateTo(string linkText)
        {
            await ExecuteWithFrameRetry(async () =>
            {
                if (FrameManager?.TopFrame == null)
                {
                    TestContext.WriteLine($"Error: TopFrame is not initialized. Cannot navigate to tab '{linkText}'.");
                    throw new InvalidOperationException("TopFrame must be initialized before navigating tabs.");
                }

                await Helper.NavigateModuleTabs(FrameManager.TopFrame, linkText);
            }, $"NavigateTo({linkText})");

            // Re-initialize frames after potential content change
            await ReInitialiseFrames();
        }

        /// <summary>
        /// Clicks an element within the module's options frame.
        /// </summary>
        /// <param name="elementName">The selector for the element to click within the options frame.</param>
        public async Task NavigateToOptions(string elementName)
        {
            await ExecuteWithFrameRetry(async () =>
            {
                if (FrameManager?.OptionsFrame == null)
                {
                    TestContext.WriteLine($"Error: OptionsFrame is not initialized. Cannot navigate to options element '{elementName}'.");
                    throw new InvalidOperationException("OptionsFrame must be initialized before navigating options.");
                }
                await Helper.ClickElementAsync(elementName, FrameManager.OptionsFrame);
            }, $"NavigateToOptions({elementName})");
        }

        #region Generic Frame Interaction Wrappers

        /// <summary>
        /// Navigates to a configuration tab (assuming it's in the top frame).
        /// </summary>
        /// <param name="tab">The text of the configuration tab.</param>
        public async Task NavigateToTabAsync(string tab) =>
            await NavigateTo(tab);

        /// <summary>
        /// Gets an element handle from the content frame.
        /// </summary>
        /// <param name="elementSelector">CSS or XPath selector for the element.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        /// <returns>The element handle.</returns>
        public async Task<IElementHandle> GetElement(string elementSelector, NavigatorTypes.Types type)
        {
            return await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<IElementHandle>(type, async (navigator, parameters) =>
                {
                    return await ((dynamic)navigator).GetElement((string)parameters[0]);
                }, elementSelector),
                $"GetElement({elementSelector})"
            );
        }

        /// <summary>
        /// Clicks an element in the content frame.
        /// </summary>
        /// <param name="elementSelector">CSS or XPath selector for the element.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task ClickElement(string elementSelector, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickElement((string)parameters[0]);
                    return Task.CompletedTask;
                }, elementSelector),
                $"ClickElement({elementSelector})"
            );
        }

        /// <summary>
        /// Updates the value of a text input element in the content frame.
        /// </summary>
        /// <param name="elementSelector">CSS or XPath selector for the input element.</param>
        /// <param name="updatedText">The text to fill the element with.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task UpdateTextElement(string elementSelector, string updatedText, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).UpdateTextElement((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, elementSelector, updatedText),
                $"UpdateTextElement({elementSelector})"
            );
        }

        /// <summary>
        /// Selects an option from a dropdown in the content frame by its visible text.
        /// </summary>
        /// <param name="elementSelector">CSS or XPath selector for the select element.</param>
        /// <param name="optionLabel">The visible text of the option to select.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task SelectDropDownElement(string elementSelector, string optionLabel, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).SelectDropDownElement((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, elementSelector, optionLabel),
                $"SelectDropDownElement({elementSelector})"
            );
        }

        /// <summary>
        /// Clicks an element in the row of a table.
        /// </summary>
        /// <param name="rowText">The title text of the row you want to click the button inside of</param>
        /// <param name="linkText">The label of the button you want to click</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task ClickRowElement(string rowText, string linkText, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickRowElement((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, rowText, linkText),
                $"ClickRowElement({rowText}, {linkText})"
            );
        }

        /// <summary>
        /// Clicks a button in a row with a specific icon
        /// </summary>
        /// <param name="rowText">The title of the row you would like to search for the button in - this is based on an EXACT match</param>
        /// <param name="imgSRC">The img src= text for identifying which button to press</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task ClickIconInRow(string rowText, string imgSRC, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickIconInRow((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, rowText, imgSRC),
                $"ClickIconInRow({rowText}, {imgSRC})"
            );
        }

        /// <summary>
        /// Clicks a button in a row with a specific icon
        /// </summary>
        /// <param name="rowText">The title of the row you would like to search for the button in - this is based on a PARTIAL match</param>
        /// <param name="imgSRC">The img src= text for identifying which button to press</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task ClickIconInRowPartialMatch(string rowText, string imgSRC, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickIconInRowPartialMatch((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, rowText, imgSRC),
                $"ClickIconInRow({rowText}, {imgSRC})"
            );
        }

        public async Task ClickIconInDivTable(string divisionName, string imgSRC, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickIconInDivTable((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, divisionName, imgSRC),
                $"ClickIconInRow({divisionName}, {imgSRC})"
            );
        }
        public async Task ClickLocatorInRow(string rowText, string elementLocator, NavigatorTypes.Types type)
        {
            await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
                {
                    await ((dynamic)navigator).ClickLocatorInRow((string)parameters[0], (string)parameters[1]);
                    return Task.CompletedTask;
                }, rowText, elementLocator),
                $"ClickIconInRow({rowText}, {elementLocator})"
            );
        }




        /// <summary>
        /// Checks if an element is currently visible on screen
        /// </summary>
        /// <param name="elementSelector">The locator of the element you would like to see is visible</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task<bool> IsElementVisible(string elementSelector, NavigatorTypes.Types type)
        {
            return await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<bool>(type, async (navigator, parameters) =>
                {
                    return await ((dynamic)navigator).IsElementVisible((string)parameters[0]);
                }, elementSelector),
                $"IsElementVisible({elementSelector})"
            );
        }

        /// <summary>
        /// Wait for the given locator to become visible before moving on.
        /// </summary>
        /// <param name="elementSelector">The locator you would like to wait for.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task<bool> WaitForElementToBeVisible(string elementSelector, NavigatorTypes.Types type)
        {
            return await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<bool>(type, async (navigator, parameters) =>
                {
                    return await ((dynamic)navigator).WaitForElementToBeVisible((string)parameters[0]);
                }, elementSelector),
                $"WaitForElementToBeVisible({elementSelector})"
            );
        }

        /// <summary>
        /// Wait for the given locator to become visible before moving on with a timeout limit.
        /// </summary>
        /// <param name="selector">The locator you would like to wait for.</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        /// <param name="timeout">The maximum time in ms you would like to wait.</param>
        public async Task WaitForSelectorAsync(string selector, NavigatorTypes.Types type, int? timeout = null)
        {
            await NavigationHandler.ExecuteNavigatorFunction<object>(type, async (navigator, parameters) =>
            {
                await ((dynamic)navigator).WaitForSelectorAsync((string)parameters[0], (int?)parameters[1]);
                return Task.CompletedTask;
            }, selector, timeout);
        }

        /// <summary>
        /// Searches for a string in every table on the page for an exact match.
        /// </summary>
        /// <param name="textToFind">The string you would like to search for</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task<IElementHandle> FindStringInTable(string textToFind, NavigatorTypes.Types type)
        {
            return await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<IElementHandle>(type, async (navigator, parameters) =>
                {
                    return await ((dynamic)navigator).FindStringInTable((string)parameters[0]);
                }, textToFind),
                $"FindStringInTable({textToFind})"
            );
        }

        /// <summary>
        /// Searches for a string in every table on the page with a partial match.
        /// </summary>
        /// <param name="textToFind">The string you would like to search for</param>
        /// <param name="type">The content navigator type you would like to use.</param>
        public async Task<IElementHandle> FindPartialStringInTable(string textToFind, NavigatorTypes.Types type)
        {
            return await ExecuteWithFrameRetry(
                () => NavigationHandler.ExecuteNavigatorFunction<IElementHandle>(type, async (navigator, parameters) =>
                {
                    return await ((dynamic)navigator).FindPartialStringInTable((string)parameters[0]);
                }, textToFind),
                $"FindStringInTable({textToFind})"
            );
        }

        #endregion

        #region *** New Window/Tab Handling ***

        /// <summary>
        /// Performs an action that triggers a new page (window/tab), waits for the new page,
        /// creates and initializes its page object using the provided factory, and returns it.
        /// </summary>
        /// <typeparam name="TPageObject">The type of the page object for the new page.</typeparam>
        /// <param name="actionTriggeringNewPage">An async function that performs the action that opens the new page.</param>
        /// <param name="pageObjectFactory">An async function that takes the new IPage instance and returns a fully initialized page object.</param>
        /// <param name="timeoutMilliseconds">Optional timeout in milliseconds to wait for the new page to open.</param>
        /// <returns>An initialized instance of the TPageObject for the new page.</returns>
        protected async Task<TPageObject> PerformActionAndWaitForNewPageAsync<TPageObject>(
            Func<Task> actionTriggeringNewPage,
            Func<IPage, Task<TPageObject>> pageObjectFactory,
            int? timeoutMilliseconds = null) where TPageObject : BasePage
        {
            if (actionTriggeringNewPage == null) throw new ArgumentNullException(nameof(actionTriggeringNewPage));
            if (pageObjectFactory == null) throw new ArgumentNullException(nameof(pageObjectFactory));

            int waitTimeout = timeoutMilliseconds ?? DefaultPopupTimeout;

            // Start waiting for the new page *before* performing the action
            var pageWaitTask = Page.Context.WaitForPageAsync(new BrowserContextWaitForPageOptions { Timeout = waitTimeout });

            TestContext.WriteLine($"Performing action and waiting for new page of type {typeof(TPageObject).Name} (Timeout: {waitTimeout}ms)");
            await actionTriggeringNewPage();
            TestContext.WriteLine("Action complete, waiting for page event...");

            IPage newPage = null;
            try
            {
                newPage = await pageWaitTask;
                TestContext.WriteLine($"New page opened: {newPage.Url}");

                // Mark current page as popup parent
                FrameStateManager.MarkAsPopupParent(Page);

                // Add a small delay or wait for a specific element if the page needs time to settle after opening
                await newPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions { Timeout = 5000 });

                TestContext.WriteLine($"Calling factory for {typeof(TPageObject).Name}");
                TPageObject initializedPageObject = await pageObjectFactory(newPage);
                TestContext.WriteLine($"{typeof(TPageObject).Name} instance created and initialized.");

                // Set up automatic parent reinitialization when popup closes - ONLY mark, don't reinitialize immediately
                newPage.Close += (sender, args) =>
                {
                    TestContext.WriteLine("Popup closed, marking parent for reinitialization on next frame operation (if needed).");
                    FrameStateManager.OnPopupClosed(Page);
                };

                return initializedPageObject;
            }
            catch (TimeoutException ex)
            {
                TestContext.WriteLine($"Timeout waiting for new page for {typeof(TPageObject).Name}");
                throw new PlaywrightException($"Timeout ({waitTimeout}ms) waiting for new page of type {typeof(TPageObject).Name} to open.", ex);
            }
            catch (Exception ex)
            {
                // Log error during factory execution or page interaction
                TestContext.WriteLine($"Error during new page handling/factory execution for {typeof(TPageObject).Name}: {ex.Message}");
                // Close the new page if it was opened but initialization failed?
                if (newPage != null && !newPage.IsClosed)
                {
                    await newPage.CloseAsync();
                }
                throw; // Re-throw the original exception
            }
        }

        #endregion
    }
}