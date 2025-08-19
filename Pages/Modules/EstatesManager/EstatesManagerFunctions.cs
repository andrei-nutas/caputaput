using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerUIMap;
using PUI = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerPopupUIMap;
using SQLUI = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerSQL;
using System.Threading.Tasks;
using System;

namespace TAF_iSAMS.Pages.Modules.EstatesManager
{
    class EstatesManagerFunctions : ModuleBasePage
    {

        private EstatesManagerFunctions(IPage page) : base(page) { }
        public SQLConnector connector = new SQLConnector();

        /// <summary>
        /// Factory method to create and initialize the School Manager page.
        /// </summary>
        public static async Task<EstatesManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating SchoolManagerFunctions instance.");
            var instance = new EstatesManagerFunctions(page);
            // Initialize the iframe hierarchy for the School Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator, Ui.ModuleContentFrame);
            Console.WriteLine($"SchoolManagerFunctions instance created and initialized.");
            return instance;
        }



        public async Task<EstatesManagerPopupFunctions> CreatePopupAsync(Func<Task> interactionAction, int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized.");
            }

            // Define the factory function for the popup
            Func<IPage, Task<EstatesManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await EstatesManagerPopupFunctions.CreateAsync(newPage, this);
            };

            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for SchoolManagerPopupFunctions.");
            return await PerformActionAndWaitForNewPageAsync(
            interactionAction,
            popupFactory,
            timeoutMilliseconds
            );
        }

        public async Task<EstatesManagerPopupFunctions> CreateSchoolBuilding()
        {
            TestContext.WriteLine("Clicking 'Create School Building' and interacting with popup.");

            try
            {
                return await CreatePopupAsync(async () =>
                {
                    TestContext.WriteLine($"Clicking button using selector: {Ui.CreateSchoolBuildingButton}");
                    await ClickElement(Ui.CreateSchoolBuildingButton, NavigatorTypes.Types.Content);
                });
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Building popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<EstatesManagerPopupFunctions> CreateSchoolClassroom()
        {
            TestContext.WriteLine("Clicking 'Create School Classroom' and interacting with popup.");

            try
            {
                return await CreatePopupAsync(async () =>
                {
                    TestContext.WriteLine($"Clicking button using selector: {Ui.CreateSchoolBuildingButton}");
                    await ClickElement(Ui.CreateSchoolClassroomButton, NavigatorTypes.Types.Content);
                });
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Building popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
        public async Task<IElementHandle> GetCreatedBuilding() =>
            await GetElement(Ui.CreatedBuildingOnPage, NavigatorTypes.Types.LeftFrame);
        public async Task<IElementHandle> GetCreatedClassroom()
        {
            await ClickElement(Ui.BuildingASelector, NavigatorTypes.Types.LeftFrame);
            return await GetElement(Ui.CreatedClassroomOnPage, NavigatorTypes.Types.LeftFrame);
        }


        public void RemoveCreatedBuilding() =>
            connector.ExecuteQuery(SQLUI.RemoveCreatedBuilding);
        public void RemoveCreatedClassroom() =>
            connector.ExecuteQuery(SQLUI.RemoveCreatedClassroom);
    }
}
