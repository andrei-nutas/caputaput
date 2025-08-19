
using System.Data;

namespace TAF_iSAMS.Pages.Modules.SchoolManager
{
    /// <summary>
    /// Contains UI selectors specific to the "Create Form" popup window.
    /// </summary>
    public static class SchoolManagerPopupUIMap
    {
        public const string SaveButton = "#btnSave";
        public const string CloseButton = "#btnCancel";

        public const string FormNameInput = "input[name='txtName']";
        public const string DummyFormName = "Test Form";
        public const string FormRoomDropDown = "id='intRoom'";
        public const string FormRoomDropDownValue = "CB";
        public const string FormTutorDropDown = "id='txtTutor'";
        public const string FormTutorDropDownValue = "ARA - Anna Adams";
        public const string AlternateFormTutorDropDownValue = "FSA - Faith Adams-Barry";
        public const string FormYearDropDown = "name='txtYear'";
        public const string FormYearDropDownValue = "Year 10 (10)";

        public const string HouseNameSelector = "input[name='txtName']";
        public const string HouseName = "Test.House.01";
        public const string HouseCodeSelector = "input[name='txtCode']";
        public const string HouseCode = "TH01";
        public const string HouseMasterDropDown = "name='txtHM'";
        public const string HouseMasterDropDownValue = "ARA - Anna Adams";
        public const string HouseMasterInitials = "ARA";
        public const string HouseGenderMixed = "input[id='mi']";
        public const string HouseGenderMale = "input[id='m']";
        public const string HouseGenderFemale = "input[id='f']";
        public const string HouseTypeBoarding = "input[id='Boarding']";
        public const string HouseTypeAcademic = "input[id='Academic']";

        public const string TutorDropDown = "name='txtTutor'";
        public const string TutorDropDownOption = FormTutorDropDownValue;

        public const string YearNameSelector = "input[name='txtName']";
        public const string YearName = "TestYear01";        
        public const string YearCodeSelector = "input[name='txtCode']";
        public const string YearCode = "TY01";
        public const string NCYearDropDown = "name='txtYear'";
        public const string NCYearDropDownOption = "NC Year: 30";
        public const string DfEYearGroupDropDown = "name='txtDfeYearGroup'";
        public const string DfEYearGroupDropDownOption = "Year 10";
        public const string YearGroupDropDown = "name='txtISCEnglandWalesYearGroup'";
        public const string YearGroupDropDownOption = "Year 10";
        public const string AverageStartingAgeDropDown = "name='txtAvgAgeAtheBeg'";
        public const string AverageStartingAgeDropDownOption = "15";
        public const string YearBlockNameSelector = "input[name='txtName']";
        public const string YearBlockName = "Test.Block.Name.01";
        public const string YearBlockCodeSelector = "input[name='txtCode']";
        public const string YearBlockCode = "TBN01";
        public const string YearBlockYearDropDown = "name='txtYear'";
        public const string YearBlockYearDropDownValue = "Year 14 (14)";
        public const string UpdatedYearBlockName = "ExampleYearBlock 1";

        public const string TermNameDropDown = "id='intTerm'";
        public const string TermNameDropDownValue = "Autumn";
        public const string SchoolYearSelector = "input[id='intSchoolYear']";
        public static string SchoolYear = (DateTime.Now.Year + 10).ToString();
        public static string SchoolTermNameOnScreen = TermNameDropDownValue + " (" + SchoolYear + ")";


        public const string DepartmentNameSelector = "input[id='txtName']";
        public const string DepartmentName = "Example Department";
        public const string DepartmentCodeSelector = "input[id='txtCode']";
        public const string DepartmentCode = "EXD";
        public const string DepartmentDivisionDropDown = "id='intDivision'";
        public const string DepartmentDivisionDropDownValue = "All Divisions";
        public static string DivisionNameSelector = "input[name='txtName']";
        public static string DivisionName = "Example Division";
        public static string DivisionCodeSelector = "input[name='txtCode']";
        public static string DivisionCode = "EXD";
        public static string DivisionYearGroupDropDown = "id='intYear'";
        public static string DivisionYearGroupDropDownValue = "Year 15 [Y15]";
        public static string DivisionNameInTable = "Year 15 (Y15)";
        public const string TempDivisionFullName = "Test Division [TD]";
        public const string UpdatedYearName = "EX1";
        public const string FormsTickAllPupils = "#selectall";//"input[name='selectall']"
        public const string FormAddPupilsSaveButton = "div.button:has-text('Save & Close')";
        public const string surnameFilterSelector = "b:has-text('Surname:')";
    }
}
