using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Sql = TAF_iSAMS.Pages.Modules.TimetableManager.TimetableManagerSQL;
using Ui = TAF_iSAMS.Pages.Modules.TimetableManager.TimetableManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.TimetableManager
{
    public class TimetableManagerFunctions : ModuleBasePage
    {

        public SQLConnector connector = new SQLConnector();


        private TimetableManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the Timetable Manager page.
        /// </summary>
        public static async Task<TimetableManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating TimetableManagerFunctions instance.");
            var instance = new TimetableManagerFunctions(page);
            // Initialize the iframe hierarchy for the Timetable Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator, Ui.ModuleContentFrame);
            Console.WriteLine($"TimetableManagerFunctions instance created and initialized.");
            return instance;
        }



        public async Task<TimetableManagerPopupFunctions> CreatePopupAsync(Func<Task> interactionAction, int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized.");
            }

            // Define the factory function for the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for TimetableManagerPopupFunctions.");
            return await PerformActionAndWaitForNewPageAsync(
            interactionAction,
            popupFactory,
            timeoutMilliseconds
            );
        }


        public async Task ClickManagePeriodsAndDaysButton() =>
            await ClickElement(Ui.ManagePeriodsAndDaysButton, NavigatorTypes.Types.OptionsFrame);


        public async Task<TimetableManagerPopupFunctions> CreateTimetableDayPopup()
        {
            TestContext.WriteLine("Clicking 'Create a Timetable Day' and interacting with popup.");

            TimetableManagerPopupFunctions? createTimetableDayPopup = null;
            try
            {
                return createTimetableDayPopup = await CreateTimetableDayAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Timetable Day popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }


        public async Task<TimetableManagerPopupFunctions> CreateTimetableDayAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Timetable Day button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking the Save button using selector: {Ui.CreateADayButton}");
                // Use ContentNavigator to interact within the correct frame
                await ReInitialiseFrames();
                await ClickElement(Ui.CreateADayButton, NavigatorTypes.Types.RightFrame);

            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateTimetableDayFunction.");
            return await PerformActionAndWaitForNewPageAsync<TimetableManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<TimetableManagerPopupFunctions> CreateTimetableWeekAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Timetable Week button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking the Save button using selector: {Ui.CreateAWeekButton}");
                // Use ContentNavigator to interact within the correct frame
                await ReInitialiseFrames();
                await ClickElement(Ui.CreateAWeekButton, NavigatorTypes.Types.RightFrame);

            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateTimetableWeekFunction.");
            return await PerformActionAndWaitForNewPageAsync<TimetableManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<TimetableManagerPopupFunctions> CreateTimetablePeriodAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Timetable Period button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking the Save button using selector: {Ui.CreateAPeriodButton}");
                // Use ContentNavigator to interact within the correct frame
                await ReInitialiseFrames();
                await ClickElement(Ui.CreateAPeriodButton, NavigatorTypes.Types.RightFrame);

            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateTimetablePeriodFunction.");
            return await PerformActionAndWaitForNewPageAsync<TimetableManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<TimetableManagerPopupFunctions> EditTimetableWeekAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Edit Timetable Week button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {

                await ReInitialiseFrames();
                await ClickElement(Ui.EditAWeekButton, NavigatorTypes.Types.RightFrame);

            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            return await PerformActionAndWaitForNewPageAsync<TimetableManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<TimetableManagerPopupFunctions> EditTimetableDayAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Edit Timetable Day button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {

                await ReInitialiseFrames();
                await ClickElement(Ui.EditADayButton, NavigatorTypes.Types.RightFrame);

            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<TimetableManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await TimetableManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            return await PerformActionAndWaitForNewPageAsync<TimetableManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }



        public async Task<IElementHandle> GetCreatedTimetableDay()
        {
            await ReInitialiseFrames();
            await ClickElement(Ui.CreateADayAppears, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.CreateADayAppears, NavigatorTypes.Types.LeftFrame);
        }


        public async Task<IElementHandle> GetCreatedTimetableWeek()
        {
            await ReInitialiseFrames();
            await ClickElement(Ui.CreateAWeekAppears, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.CreateAWeekAppears, NavigatorTypes.Types.LeftFrame);
        }

        public async Task<IElementHandle> GetSQLCreatedTimetableDay()
        {
            await ReInitialiseFrames();
            await ClickElement(Ui.CreateAWeekAppears, NavigatorTypes.Types.LeftFrame);
            await ClickElement(Ui.CreateADayAppears, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.CreateADayAppears, NavigatorTypes.Types.LeftFrame);
        }


        public async Task<IElementHandle> GetEditedTimetableWeek()
        {
            await ReInitialiseFrames();
            await ClickElement(Ui.EditedWeekAppears, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.EditedWeekAppears, NavigatorTypes.Types.LeftFrame);
        }


        public async Task<IElementHandle> GetEditedTimetableDay()
        {
            await ReInitialiseFrames();
            await ClickElement(Ui.CreateAWeekAppears, NavigatorTypes.Types.LeftFrame);
            await ClickElement(Ui.EditedDayAppears, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.EditedDayAppears, NavigatorTypes.Types.LeftFrame);
        }


        public async Task<IElementHandle> GetCreatedTimetablePeriod()
        {
            await ReInitialiseFrames();

            await Page.WaitForTimeoutAsync(1000); // Allow popup rendering time
            await ClickElement(Ui.ClickTimetableWeekInTree, NavigatorTypes.Types.LeftFrame);

            await Page.WaitForTimeoutAsync(1000); // Allow popup rendering time
            await ClickElement(Ui.ClickTimetableDayInTree, NavigatorTypes.Types.LeftFrame);

            await Page.WaitForTimeoutAsync(1000); // Allow popup rendering time
            await ClickElement(Ui.CreateAPeriodAppears, NavigatorTypes.Types.LeftFrame);

            return await GetElement(Ui.CreateAPeriodAppears, NavigatorTypes.Types.LeftFrame);
        }


            public void DeleteTimetableDay() =>
            connector.ExecuteQuery(Sql.SqlDeleteTimetableDay);


        public void DeleteTimetableWeek() =>
            connector.ExecuteQuery(Sql.SqlDeleteTimetableWeek);


        public void DeleteEditedTimetableWeek() =>
            connector.ExecuteQuery(Sql.SqlDeleteEditedTimetableWeek);

        public void DeleteEditedTimetableDay() =>
            connector.ExecuteQuery(Sql.SqlDeleteEditedTimetableWeek);

        public void DeleteTimetablePeriod() =>
            connector.ExecuteQuery(Sql.SqlDeleteTimetablePeriod);

        public void AddTimetableWeekSQL() =>
            connector.ExecuteQuery(Sql.SqlAddTimetableWeek);

        public void AddTimetableDaySQL() =>
            connector.ExecuteQuery(Sql.SqlAddTimetableDay);


    }
}
