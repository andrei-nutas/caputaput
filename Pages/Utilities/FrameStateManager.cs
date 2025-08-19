using Microsoft.Playwright;
using System.Collections.Concurrent;

namespace TAF_iSAMS.Pages.Utilities
{
    /// <summary>
    /// Manages frame state and automatically handles reinitialization when needed
    /// </summary>
    public static class FrameStateManager
    {
        private static readonly ConcurrentDictionary<IPage, FrameState> _pageStates = new();

        public class FrameState
        {
            public bool NeedsReinitialization { get; set; } = false;
            public DateTime LastInitialized { get; set; } = DateTime.Now;
            public int FailureCount { get; set; } = 0;
            public bool IsPopupParent { get; set; } = false;
        }

        /// <summary>
        /// Marks a page as needing frame reinitialization
        /// </summary>
        public static void MarkForReinitialization(IPage page)
        {
            var state = _pageStates.GetOrAdd(page, _ => new FrameState());
            state.NeedsReinitialization = true;
            TestContext.WriteLine($"Page {page.Url} marked for frame reinitialization");
        }

        /// <summary>
        /// Marks a page as a popup parent (will need reinitialization when popup closes)
        /// </summary>
        public static void MarkAsPopupParent(IPage page)
        {
            var state = _pageStates.GetOrAdd(page, _ => new FrameState());
            state.IsPopupParent = true;
            TestContext.WriteLine($"Page {page.Url} marked as popup parent");
        }

        /// <summary>
        /// Called when a popup closes - marks the parent for reinitialization
        /// </summary>
        public static void OnPopupClosed(IPage parentPage)
        {
            MarkForReinitialization(parentPage);
            TestContext.WriteLine($"Popup closed, parent page {parentPage.Url} marked for reinitialization");
        }

        /// <summary>
        /// Checks if a page needs frame reinitialization
        /// </summary>
        public static bool NeedsReinitialization(IPage page)
        {
            if (_pageStates.TryGetValue(page, out var state))
            {
                return state.NeedsReinitialization ||
                       (state.FailureCount > 0 && DateTime.Now.Subtract(state.LastInitialized).TotalSeconds > 30);
            }
            return false;
        }

        /// <summary>
        /// Marks a page as successfully initialized
        /// </summary>
        public static void MarkAsInitialized(IPage page)
        {
            var state = _pageStates.GetOrAdd(page, _ => new FrameState());
            state.NeedsReinitialization = false;
            state.LastInitialized = DateTime.Now;
            state.FailureCount = 0;
            TestContext.WriteLine($"Page {page.Url} marked as successfully initialized");
        }

        /// <summary>
        /// Records a failure for a page
        /// </summary>
        public static void RecordFailure(IPage page)
        {
            var state = _pageStates.GetOrAdd(page, _ => new FrameState());
            state.FailureCount++;
            if (state.FailureCount >= 2)
            {
                state.NeedsReinitialization = true;
            }
            TestContext.WriteLine($"Failure recorded for page {page.Url}, failure count: {state.FailureCount}");
        }

        /// <summary>
        /// Cleans up state for closed pages
        /// </summary>
        public static void CleanupClosedPage(IPage page)
        {
            _pageStates.TryRemove(page, out _);
            TestContext.WriteLine($"Cleaned up state for closed page {page.Url}");
        }

        /// <summary>
        /// Resets failure count after successful operation
        /// </summary>
        public static void ResetFailureCount(IPage page)
        {
            if (_pageStates.TryGetValue(page, out var state))
            {
                state.FailureCount = 0;
            }
        }
    }
}