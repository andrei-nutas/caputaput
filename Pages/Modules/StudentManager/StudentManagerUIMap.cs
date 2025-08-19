using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.StudentManager
{
    internal class StudentManagerUIMap
    {

        // UI element selectors for the Student Manager page

        #region Student Manager
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Student Manager");
        #endregion

        #region Add Pupil tab
        public static string TitleDropdownField = "id='Title'";
        public static string TitleDropdownValue = "Mrs";

        public static string ForenameFieldSelector = "input[id='ForenameField']";
        public static string ForenameFieldValue = "AutomationTest";

        public static string SurnameFieldSelector = "input[id='SurnameField']";
        public static string SurnameFieldValue = "AddPupil";

        public static string DateOfBirthField = "input[id='txtDOB']";
        public static string DateOfBirthValue = "01/01/2020";

        public static string ExecuteNextButton = "div.button:has-text('Next')";

        public static string Stage1CompletedMessage = "text='Stage 1 Completed'";
        #endregion


        #region Current Pupil tab
        public static string StudentForenameSearchSelector = "input[id='Forename_text']";
        public static string StudentForenameSearchValue = "TEST";

        public static string StudentSurnameSearchSelector = "input[id='Surname_text']";
        public static string StudentSurnameSearchValue = "AUTOMATIONSEARCHCURRENT";

        public static string ExecuteSearchButton = "div.button.important:has-text('Search')";

        public static string PupilFullNameValueSearchResult = "AUTOMATIONSEARCHCURRENT, TEST (TESTAUTO)";

        public static string ViewPupilEnrolmentButton = "span:has-text('Enrolment')";

        public static string StudentMiddleNameSelector = "input[id='txtMiddleNames']";
        public static string StudentMiddleNameValue = "AUTO TEST MIDDLE NAME";

        public static string UpdateDataButton = "div.button:has-text('Update Data')";
        public static string UpdatePupilMemberMessage = "span:has-text('Pupil Updated Successfully')";

        public static string SelectAllSearchResults = "input[id='select-all']";

        #endregion


        #region Applicants tab
        public static string ApplicantForenameSearchSelector = "input[id='Forename_text']";
        public static string ApplicantForenameSearchValue = "TEST";

        public static string ApplicantSurnameSearchSelector = "input[id='Surname_text']";
        public static string ApplicantSurnameSearchValue = "AUTOMATIONSEARCH";

        public static string ApplicantFullNameValueSearchResult = "AUTOMATIONSEARCH, TEST (TESTAUTO)";

        public static string ViewApplicantFamilyButton = "span:has-text('Family')";
        #endregion



        #region Former Pupils tab
        public static string FormerPupilForenameSearchSelector = "input[id='Forename_text']";
        public static string FormerPupilForenameSearchValue = "TEST";

        public static string FormerPupilSurnameSearchSelector = "input[id='Surname_text']";
        public static string FormerPupilSurnameSearchValue = "AUTOMATIONSEARCH";

        public static string FormerPupilFullNameValueSearchResult = "AUTOMATIONSEARCH, TEST (TESTAUTO)";

        public static string ViewFormerPupilFamilyButton = "span:has-text('Family')";

        #endregion


        // Add additional UI element selectors as needed.

    }
}
