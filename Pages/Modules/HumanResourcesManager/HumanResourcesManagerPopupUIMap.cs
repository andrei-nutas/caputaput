using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.HumanResourcesManager
{
    internal class HumanResourcesManagerPopupUIMap
    {        

        SQLConnector connector = new SQLConnector();

        // UI element selectors for the Human Resources Manager page

        public const string SaveButton = "#btnSave";
        public const string CloseButton = "#btnCancel";

        public const string FieldDropDowSelector = "id='field'";
        public const string FieldDropDownValue = "School Initials - TEXT";
        public const string InitialsFieldSelector = "input[id='entry']";
        public const string InitialsUpdatedValue = "AUTOINITIAL";

        public const string SaveButtonIcon = "/Legacy/system/images/16/disk_blue.gif";

        // Add additional UI element selectors as needed.

    }
}
