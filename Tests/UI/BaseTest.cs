using Microsoft.Playwright;
using TAF_iSAMS.Pages.HomePage;
namespace TAF_iSAMS.Tests.UI
{
    /// <summary>
    /// A base test fixture to manage the Playwright lifecycle,
    /// and optionally handle auto-login if the test has a certain category.
    /// </summary>
    [TestFixture]
    public abstract class BaseTest
    {
        protected HomePageFunctions homePage { get; private set; }

        protected IPlaywright _playwright;
        protected IBrowser _browser;
        protected IBrowserContext _context;
        protected IPage _page;

        // Default environment values; override if needed, or set them from config.
        protected virtual string BaseUrl => "https://isams.local/";
        protected virtual string DefaultUsername => "TAFUser";
        protected virtual string DefaultPassword => "8euJEvAKh6fEaSPefcAbvoFNJ737";

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Console.WriteLine("Starting OneTimeSetUp");
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            Console.WriteLine("Browser launched.");
        }

        [SetUp]
        public async Task SetUp()
        {
            Console.WriteLine("Setting up test.");
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            Console.WriteLine("Context and page created.");

            // If the current test has the "AutoLogin" category, do the login now.
            if (CurrentTestHasCategory("AutoLogin"))
            {
                Console.WriteLine("AutoLogin category detected. Starting AutoLoginAsync");
                await AutoLoginAsync();
                Console.WriteLine("AutoLoginAsync completed.");
            }

            // Instantiating the HomePageFunctions class which is used in all tests.
            homePage = new HomePageFunctions(_page);
        }

        public async Task ReInitialiseHomePage()
        {
            homePage = new  HomePageFunctions(_page);
        }

        [TearDown]
        public async Task TearDown()
        {
            Console.WriteLine("Starting TearDown");
            if (_context != null)
            {
                await _context.CloseAsync();
                _context = null;
                Console.WriteLine("Context closed.");
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            Console.WriteLine("Starting OneTimeTearDown");
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
                Console.WriteLine("Browser closed.");
            }
            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
                Console.WriteLine("Playwright disposed.");
            }
        }

        /// <summary>
        /// Automatically log in by navigating to the login page and calling the login method.
        /// </summary>
        private async Task AutoLoginAsync()
        {
            Console.WriteLine($"Starting AutoLoginAsync with URL: {BaseUrl}, Username: {DefaultUsername}");
            var loginPage = new Pages.Login.LoginPageFunctions(_page);
            await loginPage.NavigateToLoginAsync(BaseUrl);
            await loginPage.LoginAsync(DefaultUsername, DefaultPassword);
            Console.WriteLine($"AutoLoginAsync completed.");

        }

        /// <summary>
        /// Check if the current running test has a specific category in NUnit.
        /// </summary>
        /// <param name="categoryName">The category to check for (e.g., "AutoLogin").</param>
        /// <returns>True if the current test has that category.</returns>
        protected bool CurrentTestHasCategory(string categoryName)
        {
            var categories = TestContext.CurrentContext.Test.Properties["Category"];
            Console.WriteLine($"Checking for category: {categoryName}. Result: {categories?.Contains(categoryName) ?? false}");
            if (categories == null) return false;
            return categories.Contains(categoryName);
        }

 
    }
}
