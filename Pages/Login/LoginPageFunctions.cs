using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Login.LoginPageUiMap;

namespace TAF_iSAMS.Pages.Login
{
    public class LoginPageFunctions : BasePage
    {
        SQLConnector connector = new SQLConnector();
        public LoginPageFunctions(IPage page) : base(page)
        {
        }

        /// <summary>
        /// Navigate to the iSAMS login URL and wait for username to appear.
        /// </summary>
        public async Task NavigateToLoginAsync(string url)
        {
            connector.CreateTestUser();
            connector.DisableSimultaneousLogins();
            Console.WriteLine($"Navigating to login page at URL: {url}");
            await Page.GotoAsync(url);
            Console.WriteLine($"Navigated to login page at URL: {url}");
            await Page.WaitForSelectorAsync(Ui.UsernameSelector);
            Console.WriteLine($"Username selector visible at URL: {url}");
        }

        /// <summary>
        /// Perform the login action using the provided username and password.
        /// </summary>
        public async Task LoginAsync(string username, string password)
        {
            // Fill in username
            Console.WriteLine($"Attempting login.");
            await Page.Locator(Ui.UsernameSelector).FillAsync(username);
            Console.WriteLine($"Username filled in.");

            // Fill in password
            await Page.Locator(Ui.PasswordSelector).FillAsync(password);
            Console.WriteLine($"Password filled in.");

            // Click on the login button
            await Page.Locator(Ui.LoginButtonSelector).ClickAsync();
            Console.WriteLine($"Login button clicked.");

            // If there's a "simultaneous login" prompt, handle it
            var overrideButton = Page.Locator(Ui.SimultaneousLoginButtonSelector);
            try
            {
                // Wait for the button to be visible for up to 2 seconds.
                await overrideButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 2000, State = WaitForSelectorState.Visible });
                await overrideButton.ClickAsync();
                Console.WriteLine("Simultaneous login prompt handled.");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Override button not found within 2 seconds, moving on.");
            }
        }
    }
}
