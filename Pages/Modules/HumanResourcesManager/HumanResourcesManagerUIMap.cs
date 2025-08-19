using Azure.Core;
using System;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.HumanResourcesManager

{

    internal class HumanResourcesManagerUIMap

    {

        // UI element selectors for the Human Resources Manager page

        #region Human Resources Manager
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Human Resources Manager");
        #endregion

        #region Add Staff tab
        public static string SchoolInitialsSelector = "input[id='schoolinitialsfield']";
        public static string SchoolInitialsValue = "ATHRAS";

        public static string ForenameFieldSelector = "input[id='ForenameField']";
        public static string ForenameFieldValue = "AutomationTest";

        public static string SurnameFieldSelector = "input[id='SurnameField']";
        public static string SurnameFieldValue = "HRAddStaff";

        public static string TitleDropdownField = "id='Title'";
        public static string TitleDropdownValue = "Mrs";

        public static string StatusDropdownField = "id='Status'";
        public static string StatusDropdownValue = "At School";

        public static string ExecuteNextStepButton = "div.button:has-text('Next Step')";

        public static string NewStaffCreatedSuccessMessage = "span:has-text('New staff member added successfully')";
        #endregion

        #region Manage Staff tab
        public static string StaffForenameSearchSelector = "input[id='Firstname_text']";
        public static string StaffForenameSearchValue = "AutomationSearchTest";

        public static string StaffSurnameSearchSelector = "input[id='Surname_text']";
        public static string StaffSurnameSearchValue = "HRFirstnameTest";

        public static string ExecuteSearchButton = "div.button.important:has-text('Search')";
        public static string StaffFullNameValueSearchResult = "AutomationSearchTest HRFIRSTNAMETEST";

        public static string ViewStaffEnrolmentButton = "span:has-text('Enrolment')";

        public static string StaffSchoolInitialsSelector = "input[id='InitialsField']";
        public static string StaffSchoolInitialsValue = "AUTOTEST_XYZ";

        public static string UpdateStaffMemberMessage = "span:has-text('Staff Member Updated')";
        public static string UpdateDataButton = "div.button:has-text('Update Data')";

        public static string GroupEditDropDownSelector = "id='options'";
        public static string GroupEditDropDownSelectorOption = "Group Edit";

        public static string SelectAllSearchResults = "input[id='select-all']";

        #endregion


        // Add additional UI element selectors as needed.

    }

}
