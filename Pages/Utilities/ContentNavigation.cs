using Microsoft.Playwright;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Modules;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Helper = TAF_iSAMS.Pages.Utilities.HelperFunctions;

namespace TAF_iSAMS.Pages.Utilities
{
    public class ContentNavigation
    {
        // Make the Frame property public or internal to access it from Page Objects
        public IFrame Frame { get; } // Changed from _contentFrame and made public getter
        public IPage Page { get; }

        // Reference to the ModuleBasePage for automatic reinitialization - ONLY used when frames become stale
        private readonly ModuleBasePage _moduleBasePage;

        public ContentNavigation(IFrame? iFrame = null, IPage? page = null, ModuleBasePage moduleBasePage = null)
        {
            // Store the frame instance. Handle null case gracefully.
            if (iFrame != null)
                Frame = iFrame;

            if (Frame == null && iFrame != null)
            {
                // Log or handle the case where the frame provided is null
                TestContext.WriteLine($"Warning: ContentNavigation initialized with a null IFrame.");
            }

            if (page != null)
                Page = page;

            _moduleBasePage = moduleBasePage;
        }

        // Simple method to execute operation with automatic retry - checks popup parent state and actual frame detachment
        private async Task<T> ExecuteWithMinimalRetry<T>(Func<Task<T>> operation, string operationName)
        {
            if (_moduleBasePage != null)
            {
                return await _moduleBasePage.ExecuteWithFrameRetry(operation, operationName);
            }

            // Fallback for cases without ModuleBasePage reference
            try
            {
                return await operation();
            }
            catch (Exception ex) when (IsFrameDetachedError(ex))
            {
                TestContext.WriteLine($"Frame detached error detected for {operationName} - no ModuleBasePage reference for auto-retry");
                throw; // Re-throw since we can't auto-recover
            }
        }

        private async Task ExecuteWithMinimalRetry(Func<Task> operation, string operationName)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                await operation();
                return true;
            }, operationName);
        }

        // Only detect actual frame detachment errors, not timeouts
        // ENHANCED: Also detect popup-related frame issues
        private static bool IsFrameDetachedError(Exception ex)
        {
            var message = ex.Message.ToLowerInvariant();
            return message.Contains("frame has been detached") ||
                   message.Contains("execution context was destroyed") ||
                   message.Contains("target closed") ||
                   message.Contains("page has been closed") ||
                   message.Contains("target page, context or browser has been closed");
        }

        /// <summary>
        /// Waits for a selector within the managed frame.
        /// </summary>
        /// <param name="selector">The selector string.</param>
        /// <param name="timeout">Optional timeout in milliseconds.</param>
        /// <returns>The element handle if found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the Frame is null.</exception>

        public async Task<IElementHandle?> WaitForSelectorAsync(string selector, int? timeout = null)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null)
                    throw new InvalidOperationException("Cannot wait for selector, both frame and page are null.");

                var isPage = Page != null;
                var contextName = isPage ? "page" : $"frame: {Frame?.Name}";
                TestContext.WriteLine($"Waiting for selector: {selector} in {contextName}");

                if (isPage)
                {
                    PageWaitForSelectorOptions? options = null;
                    if (timeout.HasValue)
                    {
                        options = new PageWaitForSelectorOptions { Timeout = timeout.Value };
                    }

                    return await Page.WaitForSelectorAsync(selector, options);
                }
                else
                {
                    FrameWaitForSelectorOptions? options = null;
                    if (timeout.HasValue)
                    {
                        options = new FrameWaitForSelectorOptions { Timeout = timeout.Value };
                    }

                    return await Frame.WaitForSelectorAsync(selector, options);
                }
            }, $"WaitForSelectorAsync({selector})");
        }

        public async Task<ElementResult> GetElement(string elementName)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.GetElement uses the frame correctly
                var returnedElement = await Helper.GetElement(elementName, Frame, Page);
                return returnedElement;
            }, $"GetElement({elementName})");
        }

        public async Task ClickElement(string locator)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                await Helper.ClickElementAsync(locator, Frame, Page);
            }, $"ClickElement({locator})");
        }

        public async Task SelectDropDownElement(string elementName, string option)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.SelectDropdownOptionAsync uses the frame correctly
                await Helper.SelectDropdownOptionAsync(elementName, option, Frame, Page);
            }, $"SelectDropDownElement({elementName}, {option})");
        }

        public async Task UpdateTextElement(string element, string updatedText)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.UpdateTextElementAsync uses the frame correctly
                await Helper.UpdateTextElementAsync(element, updatedText, Frame, Page);
            }, $"UpdateTextElement({element}, {updatedText})");
        }

        public async Task ClickRowElement(string rowText, string linkText)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementInRow uses the frame correctly
                await Helper.ClickElementInRow(rowText, linkText, Frame, Page);
            }, $"ClickRowElement({rowText}, {linkText})");
        }

        public async Task<bool> IsElementVisible(string locator)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                return await Helper.IsElementVisible(locator, Frame, Page);
            }, $"IsElementVisible({locator})");
        }

        public async Task<bool> WaitForElementToBeVisible(string locator, int timeout = 30000)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                return await Helper.WaitForElementToBeVisible(locator, Frame, Page);
            }, $"WaitForElementToBeVisible({locator})");
        }

        public async Task<IElementHandle> FindStringInTable(string textToFind)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                return await Helper.FindStringInTable(textToFind, Frame, Page);
            }, $"FindStringInTable({textToFind})");
        }

        public async Task<IElementHandle> FindPartialStringInTable(string textToFind)
        {
            return await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                return await Helper.FindPartialStringInTable(textToFind, Frame, Page);
            }, $"FindStringInTable({textToFind})");
        }

        public async Task ClickIconInRow(string rowText, string iconSRC)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                await Helper.ClickIconInRow(rowText, iconSRC, Frame, Page);
            }, $"ClickIconInRow({rowText}, {iconSRC})");
        }


        public async Task ClickIconInRowPartialMatch(string rowText, string iconSRC)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                await Helper.ClickIconInRowPartialMatch(rowText, iconSRC, Frame, Page);
            }, $"ClickIconInRow({rowText}, {iconSRC})");
        }



        public async Task ClickIconInDivTable(string divText, string iconSRC)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                await Helper.ClickIconInDivTable(divText, iconSRC, Frame, Page);
            }, $"ClickIconInRow({divText}, {iconSRC})");
        }

        public async Task ClickLocatorInRow(string rowText, string elementLocator)
        {
            await ExecuteWithMinimalRetry(async () =>
            {
                if (Frame == null && Page == null) throw new InvalidOperationException("Cannot select dropdown element, the underlying frame or page is null.");
                // Assuming Helper.ClickElementAsync uses the frame correctly
                await Helper.ClickLocatorInRow(rowText, elementLocator, Frame, Page);
            }, $"ClickIconInRow({rowText}, {elementLocator})");
        }
    }
}