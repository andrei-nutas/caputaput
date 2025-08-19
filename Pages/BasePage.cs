using Microsoft.Playwright;

namespace TAF_iSAMS.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }

        public BasePage(IPage page)
        {
            Page = page;
        }

        /// <summary>
        /// Allows controlled access to the Page instance for specific operations.
        /// </summary>
        /// <param name="action">The action to perform with the Page instance.</param>
        internal void WithPage(Action<IPage> action)
        {
            action(Page);
        }

        /// <summary>
        /// Allows controlled access to the Page instance for specific operations that return a value.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="func">The function to perform with the Page instance.</param>
        /// <returns>The result of the function.</returns>
        internal T WithPage<T>(Func<IPage, T> func)
        {
            return func(Page);
        }

        /// <summary>
        /// Allows controlled access to the Page instance for specific asynchronous operations.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="func">The asynchronous function to perform with the Page instance.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task<T> WithPageAsync<T>(Func<IPage, Task<T>> func)
        {
            return await func(Page);
        }
    }
}