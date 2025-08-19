using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.CensusManager
{
    class CensusManagerUIMap
    {
        #region Census Manager
        public const string AddAdmissionButton = "#addCensusButton";
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Census Manager");
        #endregion

        #region Configuration > Manage Census Settings
        public static string SchoolTypeDropDown = "select[id=lbSchoolType]";
        public static string SchoolTypeIndependent = "Independent School";
        public static string SchoolTypeNonIndependent = "Non-Independent School";
        public static string SaveSettingsButtons = "div:has-text('Save Settings')";
        #endregion

        #region Build Tab
        public static string StartISCCensusRowText = "ISC Annual Census 2025";
        public static string StartSchoolLevelAnnualCensusRowText = "School Level Annual School Census 2025";
        public static string StartSchoolCensusRowText = "School Census 2024/25"; //using part of the name because sufix may vary
        public static string StartWorkforceCensusRowText = "School Workforce Census 2024";
        public static string StartCensusButton = "Start Census";
        #endregion

        #region Census 
        public static string ValidateCensusButton = "span:has-text('Validate Census')";
        public static string ViewAndDownloadCensusReportButton = "span:has-text('View Report and Download Census')";
        public static string ViewSummaryReportAndDownloadCensusButton = "span:has-text('View Summary Report and Download Census')";
        public static string BoardingHouseIndicator = "text='Boarding House'";
        #endregion

        #region SQL Commands
        public const string SQLSchoolTypeIndependent = "update TblCensusManagerSchoolCensusConfiguration SET intSchoolType = 1";
        public const string SQLSchoolTypeNonIndependent = "update TblCensusManagerSchoolCensusConfiguration SET intSchoolType = 2";
        #endregion

        //copemtency matrix for automation engineer//
    }
}
