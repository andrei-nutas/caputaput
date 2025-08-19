namespace TAF_iSAMS.Pages.Utilities
{
    public static class ModuleSelectorHelper
    {


        private static SQLConnector sqlConnector = new SQLConnector();


        static ModuleSelectorHelper()
        {
            sqlConnector.GetModuleInfo();
        }

        /// <summary>
        /// This function will get the module ID by name using the dictionary inside the sqlConnector.
        /// </summary>
        /// <param name="moduleName">The name of the module you would like to search for</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetModuleNr(string moduleName)
        {
            foreach (var module in sqlConnector.Modules)
            {
                if (module.Value.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    return module.Key.ToString();
                }
            }

            throw new ArgumentException($"No selector found for module '{moduleName}'");
        }
    }
}
