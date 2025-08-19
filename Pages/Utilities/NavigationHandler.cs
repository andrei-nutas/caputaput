namespace TAF_iSAMS.Pages.Utilities
{
    public static class NavigationHandler
    {
        public static ContentNavigation? ContentNavigator { get; set; }
        public static ContentNavigation? NestedContentNavigator { get; set; }
        public static ContentNavigation? NestedTopFrameNavigator { get; set; }
        public static ContentNavigation? AdmissionsRecordNavigator { get; set; }
        public static ContentNavigation? AdmissionsNestedRecordNavigator { get; set; }
        public static ContentNavigation? DialogBodyNavigator { get; set; }
        public static ContentNavigation? AdmissionsFrameDataNavigator { get; set; }
        public static ContentNavigation? AdmissionsFrameResultsNavigator { get; set; }
        public static ContentNavigation? PageNavigator { get; set; }
        public static ContentNavigation? NavigatorTxtFrameLeft { get; set; }
        public static ContentNavigation? NavigatorTxtFrameData { get; set; }
        public static ContentNavigation? RecordFrame { get; set; }
        public static ContentNavigation? txtFrameOptions { get; set; }
        public static ContentNavigation? iFrameLeft { get; set; }
        public static ContentNavigation? OptionsFrame { get; set; }
        public static ContentNavigation? RightFrame { get; set; }
        public static ContentNavigation? LeftFrame { get; set; }
        public static ContentNavigation? iFrame1 { get; set; }






        // Define a delegate for the navigator functions that return a value
        public delegate Task<object> NavigatorFunction(object navigator, params object[] parameters);

        public static async Task<T> ExecuteNavigatorFunction<T>(NavigatorTypes.Types type, NavigatorFunction function, params object[] parameters)
        {
            object navigator = type switch
            {
                NavigatorTypes.Types.Content => ContentNavigator,
                NavigatorTypes.Types.NestedContent => NestedContentNavigator,
                NavigatorTypes.Types.NestedTopFrame => NestedTopFrameNavigator,
                NavigatorTypes.Types.AdmissionsRecordFrame => AdmissionsRecordNavigator,
                NavigatorTypes.Types.AdmissionsNestedRecordFrame => AdmissionsNestedRecordNavigator,
                NavigatorTypes.Types.DialogBody => DialogBodyNavigator,
                NavigatorTypes.Types.AdmissionsDataFrame => AdmissionsFrameDataNavigator,
                NavigatorTypes.Types.AdmissionsResultsFrame => AdmissionsFrameResultsNavigator,
                NavigatorTypes.Types.Page => PageNavigator,
                NavigatorTypes.Types.txtFrameLeft => NavigatorTxtFrameLeft,
                NavigatorTypes.Types.DataFrame => NavigatorTxtFrameData,
                NavigatorTypes.Types.Record => RecordFrame,
                NavigatorTypes.Types.txtFrameOptions => txtFrameOptions,
                NavigatorTypes.Types.OptionsFrame => OptionsFrame,
                NavigatorTypes.Types.RightFrame => RightFrame,
                NavigatorTypes.Types.LeftFrame => LeftFrame,
                NavigatorTypes.Types.iFrame1 => iFrame1,

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (navigator == null)
            {
                throw new InvalidOperationException($"{type} Navigator is not initialized.");
            }

            // Enhanced: The actual frame retry logic is now handled within ContentNavigation itself
            // via the ModuleBasePage reference, so we just execute the function directly
            var result = await function(navigator, parameters);
            return (T)result;
        }
    }
}