using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using UIPopup = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerPopupUIMap;
using UI = TAF_iSAMS.Pages.Modules.SchoolManager.SchoolManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.SchoolManager
{
    internal class SchoolManagerSQL
    {
        public const string SQLRemoveAcademicHouse = $"delete from TblSchoolManagementHouses where txtHouseName = '{UIPopup.HouseName}'";
        public const string SQLRemoveForm = $"delete from TblSchoolManagementForms where txtForm = '{UIPopup.DummyFormName}'";
        public const string SQLRemoveYear = $"DELETE FROM TblSchoolManagementYears WHERE txtYearName = '{UIPopup.YearName}'";
        public const string SQLRemoveYearBlock = $"DELETE FROM TblSchoolManagementYearBlocks WHERE txtYearBlock = '{UIPopup.YearBlockName}'";
        public static string SQLRemoveDepartment = $"DELETE FROM TblTeachingManagerSubjectDepartments WHERE txtDepartmentName = '{UIPopup.DepartmentName}'";
        public static string SQLRemoveTerm = $"DELETE FROM TblSchoolManagementTermDates WHERE intSchoolYear = '{UIPopup.SchoolYear}'";
        public static string SQLRemoveDivision = $"DELETE FROM TblSchoolManagementSchoolDivisions WHERE txtName = '{UIPopup.DivisionName}'";
        public static string SQLCreateSchoolDepartment =
            "INSERT INTO TblTeachingManagerSubjectDepartments (txtDepartmentName,txtCode,intOrder,txtSubmitBy,txtSubmitDateTime,txtDescription,intType,blnActive,blnArchived,txtEmailAddress,txtWebsite)" +
            $"VALUES ('{UI.TempDepartmentName}','TD',NULL,'TAF_User',GETDATE(),NULL,1,1,0,NULL,NULL)";
        public static string SQLRemoveCreatedDepartment = $"DELETE FROM TblTeachingManagerSubjectDepartments WHERE txtDepartmentName = '{UI.TempDepartmentName}'";
        public static string SQLInsertNewSchoolDivision = "INSERT INTO TblSchoolManagementSchoolDivisions (txtName,txtCode,intOrder,txtSubmitBy,txtSubmitDateTime) " +
                                                          $"VALUES ('{UI.TempDivisionName}', 'TD',3,'TAF_User',GETDATE())";
        public static string SQLDeleteNewSchoolDivison = $"DELETE FROM TblSchoolManagementSchoolDivisions WHERE txtName = '{UI.TempDivisionName}'";

        public static string UpdateSchoolTerm = $"UPDATE  TblSchoolManagementTermDates SET intSchoolYear = '2033' WHERE intSchoolYear = '{UIPopup.SchoolYear}'";
        public static string CreateYearBlock = "INSERT INTO TblSchoolManagementYearBlocks (txtYearBlock, txtBlockCode, intNCYear,txtSubmitBy,txtSubmitDateTime)" +
                                                "VALUES ('ExampleYearBlock', 'EYB-1', 15, 'TAF_User', GETDATE())";

        public static string DeleteCreatedYearBlock = "DELETE FROM TblSchoolManagementYearBlocks WHERE txtYearBlock = 'ExampleYearBlock'";
        public static string DeleteUpdatedYearBlock = $"DELETE FROM TblSchoolManagementYearBlocks WHERE txtYearBlock = '{UI.UpdatedYearBlockName}'";

        public static string SQLCreateYear = "INSERT INTO TblSchoolManagementYears(intNCYear, txtYearName, txtYearTutor, txtAsstYearTutor, txtYearCode, intAvgAgeAtheBeg, txtDfeYearGroup, txtISCEnglandWalesYearGroup, txtISCNIrelandYearGroup, txtISCScotlandYearGroup, txtEmailAddress, txtWebsite, txtSubmitBy, txtSubmitDateTime, txtYearReference)" +
                                             "VALUES(48,'Example','DLA8594899036409612','AKA5118222779616013','EX',18,'13','13+', NULL, NULL,'','', NULL, NULL,'')";

        public static string SQLDeleteCreatedYear = "DELETE FROM TblSchoolManagementYears WHERE txtYearName = 'EX1'";
    }
}
