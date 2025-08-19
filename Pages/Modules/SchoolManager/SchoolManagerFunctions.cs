using Microsoft.Playwright;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerUIMap;
using PUI = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerPopupUIMap;
using UISQL = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerSQL;
using System.Threading.Tasks;
using System;

namespace TAF_iSAMS.Pages.Modules.SchoolManager
{
    class SchoolManagerFunctions : ModuleBasePage
    {

        private SchoolManagerFunctions(IPage page) : base(page) { }

        /// <summary>
        /// Factory method to create and initialize the School Manager page.
        /// </summary>
        public static async Task<SchoolManagerFunctions> CreateAsync(IPage page)
        {
            Console.WriteLine($"Creating SchoolManagerFunctions instance.");
            var instance = new SchoolManagerFunctions(page);
            // Initialize the iframe hierarchy for the School Manager module (selector for module container).
            await instance.InitializeFramesAsync(Ui.ModuleContentLocator, Ui.ModuleContentFrame);
            Console.WriteLine($"SchoolManagerFunctions instance created and initialized.");
            return instance;
        }



        public async Task<SchoolManagerPopupFunctions> CreatePopupAsync(Func<Task> interactionAction, int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized.");
            }

            // Define the factory function for the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for SchoolManagerPopupFunctions.");
            return await PerformActionAndWaitForNewPageAsync(
            interactionAction,
            popupFactory,
            timeoutMilliseconds
            );
        }



        /// <summary>
        /// Clicks the "Create Form" button, waits for the popup window,
        /// initializes the popup's page object, and returns it.
        /// Assumes the button is located within the primary content frame.
        /// </summary>
        /// <param name="timeoutMilliseconds">Optional timeout for waiting for the popup.</param>
        /// <returns>An initialized CreateFormPopupPageFunctions instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ContentNavigator is not initialized.</exception>
        public async Task<SchoolManagerPopupFunctions> ClickCreateFormButtonAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Form button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Form button using selector: {Ui.CreateFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public async Task<SchoolManagerPopupFunctions> ClickCreateTeachingDepartmentAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Form button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Form button using selector: {Ui.CreateTeachingDepartmentButton}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateTeachingDepartmentButton, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public async Task<SchoolManagerPopupFunctions> ClickEditSchoolFormAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Form button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Form button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInRow(Ui.ExistingFormNameOnScreen, Ui.EditFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public async Task<SchoolManagerPopupFunctions> ClickEditTempDepartmentAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInRow(Ui.TempDepartmentNameOnPage, Ui.EditFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public async Task<SchoolManagerPopupFunctions> ClickEditTermAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickLocatorInRow(Ui.EditTermName, Ui.EditTermButton, NavigatorTypes.Types.Content); //Ui.TempDepartmentName
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }

        public async Task<SchoolManagerPopupFunctions> ClickEditTutorAndGetPopup(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInRow(Ui.TutorSalutationAndSurnameOnPage, Ui.EditTutorIconSRC, NavigatorTypes.Types.Content); //Ui.TempDepartmentName
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<SchoolManagerPopupFunctions> ClickEditYearBlockAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditTutorIconSRC}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInRow(Ui.CreatedYearBlockNameOnPage, Ui.EditTutorIconSRC, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }

        public async Task<SchoolManagerPopupFunctions> ClickEditTempDivisionAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInDivTable(Ui.TempDivisionName + " [TD]", Ui.EditDivisionButtonIcon, NavigatorTypes.Types.Content); //Div text: 'Temp Division [TD] 1 Year Group(s)'
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for EditPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<SchoolManagerPopupFunctions> ClickEditAcademicHouseAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the edit button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking edit button using selector: {Ui.EditFormButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickIconInRow(Ui.ExistingHouseNameOnScreen, Ui.EditFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateFormPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public async Task<SchoolManagerPopupFunctions> ClickCreateTutorButtonAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Tutor button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Tutor button using selector: {Ui.CreateHouseButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateTutorButton, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateTutorPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }

        public async Task<SchoolManagerPopupFunctions> ClickCreateHouseButtonAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create House button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Form button using selector: {Ui.CreateHouseButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateHouseButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateHousePopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }

        public async Task<SchoolManagerPopupFunctions> ClickCreateYearButtonAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Year button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Form button using selector: {Ui.CreateYearButtonLocator}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateYearPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }


        public async Task<SchoolManagerPopupFunctions> ClickCreateYearBlockButtonAndGetPopupAsync(int? timeoutMilliseconds = null)
        {
            if (NavigationHandler.ContentNavigator == null)
            {
                throw new InvalidOperationException("ContentNavigator must be initialized to click the Create Year Block button.");
            }

            // Define the action that clicks the button within the ContentFrame
            Func<Task> clickAction = async () =>
            {
                TestContext.WriteLine($"Clicking Create Year Block button using selector: {Ui.CreateYearBlockButton}");
                // Use ContentNavigator to interact within the correct frame
                await ClickElement(Ui.CreateFormButtonLocator, NavigatorTypes.Types.Content);
            };

            // Define the factory function that creates and initializes the popup page object
            // It captures 'this' instance to pass as the parent reference to the popup
            Func<IPage, Task<SchoolManagerPopupFunctions>> popupFactory = async (newPage) =>
            {
                return await SchoolManagerPopupFunctions.CreateAsync(newPage, this);
            };

            // Call the generic base method to handle the popup opening and initialization
            TestContext.WriteLine("Calling PerformActionAndWaitForNewPageAsync for CreateYearBlockPopupPageFunctions.");
            return await PerformActionAndWaitForNewPageAsync<SchoolManagerPopupFunctions>(
                clickAction,
                popupFactory,
                timeoutMilliseconds // Pass the optional timeout
            );
        }
        public SQLConnector connector = new SQLConnector();

        public async Task<SchoolManagerPopupFunctions> CreateFormPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createFormPopup = null;
            try
            {
                return createFormPopup = await ClickCreateFormButtonAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateHousePopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createHousePopup = null;
            try
            {
                return createHousePopup = await ClickCreateHouseButtonAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateTutorPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createTutorPopup = null;
            try
            {
                return createTutorPopup = await ClickCreateTutorButtonAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateYearPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickCreateYearButtonAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateYearBlockPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickCreateFormButtonAndGetPopupAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
        public async Task<SchoolManagerPopupFunctions> CreateSchoolTermPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickCreateFormButtonAndGetPopupAsync(); //These calls may be consolidated as each one appears to be using #btnAdd 
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateSchoolDivisionPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickCreateFormButtonAndGetPopupAsync(); //These calls may be consolidated as each one appears to be using #btnAdd 
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> CreateSchoolDepartmentPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickCreateTeachingDepartmentAndGetPopupAsync(); //These calls may be consolidated as each one appears to be using #btnAdd 
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditSchoolFormPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditSchoolFormAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
        public async Task<SchoolManagerPopupFunctions> EditAcademicHousePopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditAcademicHouseAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditTempDepartmentPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditTempDepartmentAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditTempDivisionPopup()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditTempDivisionAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditSchoolTerm()
        {
            TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditTermAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditTutorPopup()
        {
            TestContext.WriteLine("Clicking 'Edit' icon and interacting with popup.");

            SchoolManagerPopupFunctions? createYearPopup = null;
            try
            {
                return createYearPopup = await ClickEditTutorAndGetPopup(); //These calls may be consolidated as each one is using the edit icon as a selector
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditYearBlock()
        {
            TestContext.WriteLine("Clicking 'Edit Year Block' and interacting with popup.");

            try
            {
                return await CreatePopupAsync(async () =>
                {
                    TestContext.WriteLine($"Clicking edit icon using selector: {Ui.EditTutorIconSRC}");
                    await ClickIconInRow(Ui.CreatedYearBlockNameOnPage, Ui.EditTutorIconSRC, NavigatorTypes.Types.Content);
                });
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Edit Year Block popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditYear()
        {
            TestContext.WriteLine("Clicking 'Edit Year' and interacting with popup.");

            try
            {
                return await CreatePopupAsync(async () =>
                {
                    TestContext.WriteLine($"Clicking edit icon using selector: {Ui.EditTutorIconSRC}");
                    await ClickIconInRow(Ui.CreatedYearNameOnScreen, Ui.EditTutorIconSRC, NavigatorTypes.Types.Content);
                });
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Edit Year Block popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public async Task<SchoolManagerPopupFunctions> EditFormPupils()
        {
            TestContext.WriteLine("Clicking 'Edit Year' and interacting with popup.");

            try
            {
                return await CreatePopupAsync(async () =>
                {
                    TestContext.WriteLine($"Clicking Locator In Row using selector: {Ui.FormPupilsButton}");
                    await ClickLocatorInRow(Ui.EditableForm, Ui.FormPupilsButton, NavigatorTypes.Types.Content);
                });
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to open or initialize the Edit Year Block popup: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        //public async Task<SchoolManagerPopupFunctions> EditYearBlock()
        //{
        //    TestContext.WriteLine("Clicking 'Create Form' and interacting with popup.");

        //    SchoolManagerPopupFunctions? createYearPopup = null;
        //    try
        //    {
        //        return createYearPopup = await ClickEditYearBlockAndGetPopupAsync(); //These calls may be consolidated as each one is using the edit icon as a selector
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail($"Failed to open or initialize the Create Form popup: {ex.Message}\n{ex.StackTrace}");
        //        return null;
        //    }
        //}

        public async Task UpdateSchoolDetails()
        {
            await UpdateSchoolName("Automated Test School");
            await SetFundingOptionIndependent();
            await SetBoardingTypeDay();
            await SetDFESchoolCensus();
        }
        public async Task UpdateSchoolName(string updatedText) =>
            await UpdateTextElement(Ui.SchoolNameID, updatedText, NavigatorTypes.Types.Content);
        public async Task SetFundingOptionLEA() =>
            await ClickElement(Ui.FundingTypeLEA, NavigatorTypes.Types.Content);
        public async Task SetFundingOptionIndependent() =>
            await ClickElement(Ui.FundingTypeIndependent, NavigatorTypes.Types.Content);
        public async Task SetFundingOptionFoundation() =>
            await ClickElement(Ui.FundingTypeFoundation, NavigatorTypes.Types.Content);
        public async Task SetBoardingTypeDay() =>
            await ClickElement(Ui.BoardingTypeDay, NavigatorTypes.Types.Content);
        public async Task SetBoardingTypeBoarding() =>
            await ClickElement(Ui.BoardingTypeBoarding, NavigatorTypes.Types.Content);
        public async Task SetGenderMakeupBoys() =>
            await ClickElement(Ui.GenderMakeupBoys, NavigatorTypes.Types.Content);
        public async Task SetGenderMakeupGirls() =>
            await ClickElement(Ui.GenderMakeupGirls, NavigatorTypes.Types.Content);
        public async Task SetGenderMakeupCoEducational() =>
            await ClickElement(Ui.GenderMakeupCoEducational, NavigatorTypes.Types.Content);
        public async Task SetDFESchoolCensus() =>
            await ClickElement(Ui.DFESchoolCensus, NavigatorTypes.Types.Content);
        public async Task SetDFESchoolWorkforceCensus() =>
            await ClickElement(Ui.DFESchoolWorkforceCensus, NavigatorTypes.Types.Content);
        public async Task<IElementHandle> GetFormOnPage()
        {
            await ReInitialiseFrames();
            return await FindStringInTable(Ui.FormNameSelector, NavigatorTypes.Types.Content);
        }

        public async Task<bool> WaitForFormsToBeVisible() =>
           await WaitForElementToBeVisible(Ui.FormNameHeader, NavigatorTypes.Types.Content);
        public void DeleteCreatedSchoolForm() =>
             connector.ExecuteQuery(UISQL.SQLRemoveForm);
        public async Task<bool> WaitForHouseToBevisible() =>
            await WaitForElementToBeVisible(Ui.HouseName, NavigatorTypes.Types.Content);
        public async Task<IElementHandle> GetHouseOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.HouseName, NavigatorTypes.Types.Content);
        }
        public async Task DeleteTutorAndConfirm()
        {
            await ClickIconInRow(Ui.TutorSalutationAndSurnameOnPage, Ui.DeleteTutorIconSRC, NavigatorTypes.Types.Content);
            await ConfirmDeleteTutor();
            //await ReInitialiseFrames(); now automatically if needed
        }
        public async Task<IElementHandle> GetTutorOnPage()
        {
            await Task.Delay(1000);
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.TutorSalutationAndSurnameOnPage, NavigatorTypes.Types.Content);
        }


        public async Task<IElementHandle> GetAlternativeTutorOnPage()
        {
            await Task.Delay(1000);
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.AlternativeTutorSalutationAndSurname, NavigatorTypes.Types.Content);
        }

        public async Task DeleteAlternativeTutorAndConfirm()
        {
            await ClickIconInRow(Ui.AlternativeTutorNameOnScreen, Ui.DeleteTutorIconSRC, NavigatorTypes.Types.Content);
            await ConfirmDeleteTutor();
            //await ReInitialiseFrames(); now automatically if needed
        }
        public async Task ConfirmDeleteTutor() =>
            await ClickElement(Ui.ConfirmDeleteTutor, NavigatorTypes.Types.DialogBody);

        public async Task CancelDeleteTutor() =>
            await ClickElement(Ui.CancelDeleteTutor, NavigatorTypes.Types.DialogBody);

        public void DeleteCreatedAcademicHouse() =>
            connector.ExecuteQuery(UISQL.SQLRemoveAcademicHouse);

        public async Task<IElementHandle> GetYearOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.YearGroupName, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetYearBlockOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.YearBlockName, NavigatorTypes.Types.Content);
        }
        public async Task<IElementHandle> GetUpdatedYearBlockOnPage()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.UpdatedYearBlockName, NavigatorTypes.Types.Content);
        }
        public async Task<IElementHandle> GetUpdatedYear()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(Ui.UpdatedCreatedYearNameOnScreen, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetSchoolTerm()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(PUI.SchoolYear, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetSchoolDepartment()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(PUI.DepartmentName, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetSchoolDivision()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(PUI.DivisionNameInTable, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetSchoolFormRoom()
        {
            //await ReInitialiseFrames(); now automatically if needed
            return await FindStringInTable(PUI.FormRoomDropDownValue, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetAcademicHouseMaster()
        {
            await ReInitialiseFrames();
            return await FindStringInTable(PUI.HouseMasterInitials, NavigatorTypes.Types.Content);
        }
        public async Task<IElementHandle> GetTempSchoolDepartment()
        {
            await ReInitialiseFrames();
            return await FindStringInTable(PUI.DepartmentCode, NavigatorTypes.Types.Content);
        }

        public async Task<IElementHandle> GetTempSchoolDivision()
        {
            await ReInitialiseFrames();
            return await FindStringInTable(PUI.DivisionNameInTable, NavigatorTypes.Types.Content);
        }
        public void DeleteCreatedSchoolYear() =>
            connector.ExecuteQuery(UISQL.SQLRemoveYear);
        public void DeleteCreatedYearBlock() =>
            connector.ExecuteQuery(UISQL.SQLRemoveYearBlock);

        public void DeleteCreatedSchoolTerm() =>
            connector.ExecuteQuery(UISQL.SQLRemoveTerm);

        public void DeleteCreatedSchoolDepartment() =>
            connector.ExecuteQuery(UISQL.SQLRemoveDepartment);
        public void DeleteCreatedSchoolDivision() =>
            connector.ExecuteQuery(UISQL.SQLRemoveDivision);

        public void CreateTempDepartment() =>
            connector.ExecuteQuery(UISQL.SQLCreateSchoolDepartment);

        public void DeleteTempDepartment() =>
            connector.ExecuteQuery(UISQL.SQLRemoveCreatedDepartment);


        public void CreateTempDivision() =>
            connector.ExecuteQuery(UISQL.SQLInsertNewSchoolDivision);
        public void DeleteTempDivision() =>
            connector.ExecuteQuery(UISQL.SQLDeleteNewSchoolDivison);
        public void RevertSchoolTerm() =>
            connector.ExecuteQuery(UISQL.UpdateSchoolTerm);

        public void CreateYearBlock() =>
            connector.ExecuteQuery(UISQL.CreateYearBlock);
        public void DeleteUpdatedYearBlock() =>
         connector.ExecuteQuery(UISQL.DeleteUpdatedYearBlock);

        public void CreateYear() =>
         connector.ExecuteQuery(UISQL.SQLCreateYear);

        public void DeleteCreatedYear() =>
            connector.ExecuteQuery(UISQL.SQLDeleteCreatedYear);

    }
}
