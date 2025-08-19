using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using UI = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerPopupUIMap;


namespace TAF_iSAMS.Pages.Modules.SchoolManager
{
    internal class SchoolManagerUIMap
    {
        public const string AddSchoolManagerButton = "#addSchoolManagerButton";
        public const string DefaultSchoolTextBox = $"input[name='txtSchoolName']";
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("School Manager");
        public static string ModuleContentFrame = "txtFrameContent";
        public static string SchoolNameID = "input[name='txtSchoolName']";
        public static string FundingTypeLEA = "input[id='LEA']";
        public static string FundingTypeIndependent = "input[id='Independent']";
        public static string FundingTypeFoundation = "input[id='Foundation']";
        public static string BoardingTypeDay = "input[id='Day']";
        public static string BoardingTypeBoarding = "input[id='Boarding']";
        public static string GenderMakeupBoys = "input[id='Boys']";
        public static string GenderMakeupGirls = "input[id='Girls']";
        public static string GenderMakeupCoEducational = "input[id='Co-Educational']";
        public static string DFESchoolCensus = "input[id='chkPupilCensusShowStatus']";
        public static string DFESchoolWorkforceCensus = "input[id='chkSchoolCensusShowStatus']";
        public static string FormNameSelector = UI.DummyFormName;
        public const string FormNameHeader = "text='Form Name (Teachers)'";
        public const string CreateFormButtonLocator = "#btnAdd"; //All use the same button ID but keeping seperate for now.
        public const string EditFormButtonLocator = "/Legacy/system/images/16/document_edit.gif";
        public const string ExistingFormName = "Y-2-A";
        public const string ExistingFormNameOnScreen = "Y-2-A (-2)";
        public const string CreateHouseButtonLocator = "#btnAdd";//All use the same button ID but keeping seperate for now.
        public const string CreateYearButtonLocator = "#btnAdd";//All use the same button ID but keeping seperate for now.
        public const string CreateTutorButton = "#btnAdd";//All use the same button ID but keeping seperate for now.
        public const string CreateYearBlockButton = "#btnAdd";//All use the same button ID but keeping seperate for now.
        public const string CreateTeachingDepartmentButton = "#btnTAdd";//All use the same button ID but keeping seperate for now.
        public const string CreateNonTeachingDepartmentButton = "#btnNTAdd";//All use the same button ID but keeping seperate for now.
        public const string HouseName = UI.HouseName;
        public const string ExistingHouseName = "Academic House B";
        public const string ExistingHouseNameOnScreen = "Academic House B (AHB)";
        public const string TutorName = UI.FormTutorDropDownValue;
        public const string TutorSalutationAndSurname = "Mrs Adams";
        public const string TutorSalutationAndSurnameOnPage = "Mrs Adams (ARA)";
        public const string AlternativeTutorSalutationAndSurname = "Judge Adams-Barry";
        public const string AlternativeTutorNameOnScreen = "Judge Adams-Barry (FSA)";
        public const string DeleteTutorIconSRC = "/Legacy/system/images/16/document_delete.gif";
        public const string EditTutorIconSRC = "/Legacy/system/images/16/document_edit.gif";
        public const string ConfirmDeleteTutor = "div.button:has-text('Yes')";
        public const string CancelDeleteTutor = "div.button:has-text('Yes')";
        public const string YearGroupName = UI.YearName;
        public const string YearBlockName = UI.YearBlockName;
        public const string TempDepartmentName = "TempDepartment";
        public const string TempDepartmentNameOnPage = TempDepartmentName + " (TD)";

        public const string TempDivisionName = "Temp Division";
        public const string EditDivisionButtonIcon = "/Legacy/system/images/16/branch_element.gif";
        public const string EditTermName = "Autumn (2033)";
        public const string EditTermButton = "input[name='Edit']";

        public const string CreatedYearBlockName = "ExampleYearBlock";
        public const string CreatedYearBlockNameOnPage = "ExampleYearBlock (15)";
        public const string UpdatedYearBlockName = UI.UpdatedYearBlockName;

        public const string CreatedYearNameOnScreen = "Example (EX | 48)";
        public const string UpdatedCreatedYearNameOnScreen = UI.UpdatedYearName + " (EX | 48)";
        public const string FormPupilsButton = "input[name='Pupils']";
        public const string EditableForm = "Y-2-A (-2)";
    }
}
