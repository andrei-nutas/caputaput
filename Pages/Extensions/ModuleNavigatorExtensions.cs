using TAF_iSAMS.Pages.HomePage;
using TAF_iSAMS.Pages.Modules;
using TAF_iSAMS.Pages.Modules.AdmissionsManager;
using TAF_iSAMS.Pages.Modules.CensusManager;
using TAF_iSAMS.Pages.Modules.ReportsManager;
using TAF_iSAMS.Pages.Modules.SchoolManager;
using TAF_iSAMS.Pages.Modules.HumanResourcesManager;
using TAF_iSAMS.Pages.Modules.StudentManager;
using TAF_iSAMS.Pages.Modules.TimetableManager;
using TAF_iSAMS.Pages.Modules.EstatesManager;


namespace TAF_iSAMS.Pages.Extensions
{
    public static class ModuleNavigatorExtensions
    {
        // Dictionary mapping types to their module names
        private static readonly Dictionary<Type, string> _typeToModuleNames = new Dictionary<Type, string>
        {
            { typeof(AdmissionsManagerFunctions), "Admissions Manager" },
            // Add more mappings for other modules:
            // { typeof(SomeOtherModuleFunctions), "Other Module Name" }
            { typeof(SchoolManagerFunctions), "School Manager" },
            { typeof(ReportsManagerFunctions), "Reports Manager" },
            { typeof(CensusManagerFunctions), "Census Manager" },
            { typeof(HumanResourcesManagerFunctions), "Human Resources Manager" },
            { typeof(StudentManagerFunctions), "Student Manager" },
            { typeof(TimetableManagerFunctions), "Timetable Manager" },
            { typeof(EstatesManagerFunctions), "Estates Manager" }

        };

        /// <summary>
        /// Navigates to the module corresponding to type T and returns an initialized instance.
        /// </summary>
        /// <typeparam name="T">The type of module page to return</typeparam>
        /// <param name="homePage">The home page instance</param>
        /// <returns>An initialized instance of the module page</returns>
        public static async Task<T> NavigateToModuleAsync<T>(this HomePageFunctions homePage) where T : ModuleBasePage
        {
            Type requestedType = typeof(T);

            // Check if we have a mapping for this type
            if (!_typeToModuleNames.TryGetValue(requestedType, out string moduleName))
            {
                throw new ArgumentException($"No module name mapping found for type {requestedType.Name}");
            }

            Console.WriteLine($"Navigating to module: {moduleName} for type {requestedType.Name}");

            // Search and click the module
            await homePage.SearchAndClickModuleAsync(moduleName);
            Console.WriteLine($"Clicked on module: {moduleName}");

            // Access the page instance through the WithPageAsync method
            return await homePage.WithPageAsync(async page =>
            {
                // Create the appropriate module instance based on type
                if (typeof(T) == typeof(AdmissionsManagerFunctions))
                {
                    var instance = await AdmissionsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(EstatesManagerFunctions))
                {
                    var instance = await EstatesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(ReportsManagerFunctions))
                {
                    var instance = await ReportsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(CensusManagerFunctions))
                {
                    var instance = await CensusManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(HumanResourcesManagerFunctions))
                {
                    var instance = await HumanResourcesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(StudentManagerFunctions))
                {
                    var instance = await StudentManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(TimetableManagerFunctions))
                {
                    var instance = await TimetableManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(SchoolManagerFunctions))
                {
                    var instance = await SchoolManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Add more module types as needed:
                // if (typeof(T) == typeof(OtherModuleFunctions))
                // {
                //     var instance = await OtherModuleFunctions.CreateAsync(page);
                //     return (T)(object)instance;
                // }

                throw new ArgumentException($"No factory implementation for type {requestedType.Name}");
            });
        }

        /// <summary>
        /// Navigates to a specified module and creates an initialized instance of the corresponding page object.
        /// </summary>
        /// <typeparam name="T">The type of module page to return</typeparam>
        /// <param name="homePage">The home page instance</param>
        /// <param name="moduleName">The name of the module to navigate to</param>
        /// <returns>An initialized instance of the module page</returns>
        public static async Task<T> NavigateToModuleAsync<T>(this HomePageFunctions homePage, string moduleName) where T : ModuleBasePage
        {
            Console.WriteLine($"Navigating to module: {moduleName}");

            // Search and click the module
            await homePage.SearchAndClickModuleAsync(moduleName);
            Console.WriteLine($"Clicked on module: {moduleName}");

            // Access the page instance through the WithPageAsync method
            return await homePage.WithPageAsync(async page =>
            {
                // Handle specific module types
                if (typeof(T) == typeof(AdmissionsManagerFunctions) &&
                    moduleName.Equals("Admissions Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await AdmissionsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                // Handle specific module types
                if (typeof(T) == typeof(EstatesManagerFunctions) &&
                    moduleName.Equals("School Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await EstatesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                // Handle specific module types
                if (typeof(T) == typeof(ReportsManagerFunctions) &&
                    moduleName.Equals("Reports Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await ReportsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Handle specific module types
                if (typeof(T) == typeof(CensusManagerFunctions) &&
                    moduleName.Equals("Census Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await CensusManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Handle specific module types
                if (typeof(T) == typeof(HumanResourcesManagerFunctions) &&
                    moduleName.Equals("Human Resources Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await HumanResourcesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Handle specific module types
                if (typeof(T) == typeof(StudentManagerFunctions) &&
                    moduleName.Equals("Student Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await StudentManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Handle specific module types
                if (typeof(T) == typeof(TimetableManagerFunctions) &&
                    moduleName.Equals("Timetable Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await TimetableManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                // Handle specific module types
                if (typeof(T) == typeof(SchoolManagerFunctions) &&
                    moduleName.Equals("School Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var instance = await SchoolManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                // Add more module types as needed

                throw new ArgumentException($"No factory implementation for module '{moduleName}' and type {typeof(T).Name}");
            });
        }


        public static async Task<T> Reinitialise<T>(this HomePageFunctions homePage) where T : ModuleBasePage
        {
            Type requestedType = typeof(T);


            return await homePage.WithPageAsync(async page =>
            {
                // Create the appropriate module instance based on type
                if (typeof(T) == typeof(AdmissionsManagerFunctions))
                {
                    var instance = await AdmissionsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(EstatesManagerFunctions))
                {
                    var instance = await EstatesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(ReportsManagerFunctions))
                {
                    var instance = await ReportsManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(CensusManagerFunctions))
                {
                    var instance = await CensusManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(HumanResourcesManagerFunctions))
                {
                    var instance = await HumanResourcesManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(StudentManagerFunctions))
                {
                    var instance = await StudentManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }

                if (typeof(T) == typeof(TimetableManagerFunctions))
                {
                    var instance = await TimetableManagerFunctions.CreateAsync(page);
                    return (T)(object)instance;
                }


                // Add more module types as needed:
                // if (typeof(T) == typeof(OtherModuleFunctions))
                // {
                //     var instance = await OtherModuleFunctions.CreateAsync(page);
                //     return (T)(object)instance;
                // }

                throw new ArgumentException($"No factory implementation for type {requestedType.Name}");
            });
        }
    }
}