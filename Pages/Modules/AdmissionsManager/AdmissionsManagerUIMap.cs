using Azure.Core;
using System;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.AdmissionsManager

{

    internal class AdmissionsManagerUIMap

    {

        // UI element selectors for the Admissions Manager page

        #region Admissions Manager
        public const string AddAdmissionButton = "#addAdmissionButton";
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Admissions Manager");
        #endregion

        #region Enquiry Tab 
        public static string ApplicantForenameSelector = "input[id='txtForename']";
        public static string ApplicantForename = "TestingForename1";
        public static string ApplicantSurnameSelector = "input[id='txtSurname']";
        public static string ApplicantSurname = "TestingSurname1";
        public static string GenderDropDown = "name='txtGender'";
        public static string GenderType = "Female";
        public static string BoardingStatusDropDown = "name='txtType'";
        public static string BoardingStatusType = "Day";
        public static string AdmissionsStatusDropDown = "name='txtAdmissionsStatus'";
        public static string AdmissionsStatusType = "Enquiry";
        public static string SaveApplicantButton = "div:has-text('Save Applicant')";
        public static string ViewAuditButton = "a:has-text('View Audit')";
        #endregion

        #region Admissions Tab 
        public static string ApplicantForenameSearchSelector = "input[id='txtForename_text']";
        public static string ApplicantSurnameSearchSelector = "input[id='txtSurname_text']";
        public static string ExecuteSearchButton = "span:has-text('Execute Search')";

        public static string ApplicantForenameSearchValue = "TEST";
        public static string ApplicantSurnameSearchValue = "AUTOMATIONSEARCH";
        public static string ApplicantFullNameValue = "AUTOMATIONSEARCH, TEST (TESTAUTO)";
        #endregion

        // Add additional UI element selectors as needed.

    }

}
