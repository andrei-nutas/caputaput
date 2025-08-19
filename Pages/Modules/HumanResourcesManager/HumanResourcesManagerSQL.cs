using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI = TAF_iSAMS.Pages.Modules.HumanResourcesManager.HumanResourcesManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.HumanResourcesManager
{
    internal class HumanResourcesManagerSQL
    {

        // SQL commands for the Human Resources Manager module

        public static string SqlDeleteStaffMember = $"DELETE FROM TblStaff WHERE Fullname = 'AutomationTest HRAddStaff'";
        public static string SqlAddSearchStaffMember = $"INSERT INTO TblPerson (TblPersonUniqueId) SELECT NEWID(); INSERT INTO TblStaff (intPersonID, Initials, User_Code, Title, Firstname, Surname, Fullname, NameInitials, PreName, Salutation, LabelSalutation, LetterSalutation, Sex, SubmittedBy, SubmitTime, SubmitDate, SystemStatus, blnIsInternalExamAddResultsWizardBar, guidUniquePersonId) SELECT ((SELECT TOP (1) TblPersonId FROM tblPerson ORDER BY TblPersonId DESC)), 'XXYYZZ', 'XXYYZZ12345678901234567', 'Mrs', 'AutomationSearchTest', 'HRFirstnameTest', 'AutomationSearchTest HRFirstnameTest', 'A', 'AutomationSearchTest', 'Mrs HRAddStaff', 'Mrs A HRAddStaff', 'Mrs HRAddStaff', 'F', 'TAFUser', GETDATE(), GETDATE(), 1, 0, NEWID();";
        public static string SqlAddSecondSearchStaffMember = $"INSERT INTO TblPerson (TblPersonUniqueId) SELECT NEWID(); INSERT INTO TblStaff (intPersonID, Initials, User_Code, Title, Firstname, Surname, Fullname, NameInitials, PreName, Salutation, LabelSalutation, LetterSalutation, Sex, SubmittedBy, SubmitTime, SubmitDate, SystemStatus, blnIsInternalExamAddResultsWizardBar, guidUniquePersonId) SELECT ((SELECT TOP (1) TblPersonId FROM tblPerson ORDER BY TblPersonId DESC)), 'XXYYZZ', 'XXYYZZ123456789012345678', 'Mrs', 'AutomationSearchTest', 'HRFirstnameTest', 'AutomationSearchTest HRFirstnameTest', 'A', 'AutomationSearchTest', 'Mrs HRAddStaff', 'Mrs A HRAddStaff', 'Mrs HRAddStaff', 'F', 'TAFUser', GETDATE(), GETDATE(), 1, 0, NEWID();";
        public static string SqlDeleteSearchStaffMember = $"DELETE FROM TblStaff WHERE Fullname = 'AutomationSearchTest HRFirstnameTest'";
        public static string SqlDeleteEditedStaffMember = $"DECLARE @staffId INT; SET @staffId = (SELECT TOP (1) TblStaffId FROM TblStaff ORDER BY TblStaffID DESC); DELETE FROM TblStaffManagementCustomFieldValue WHERE StaffId = @staffId; DELETE FROM TblStaff WHERE Fullname = 'AutomationSearchTest HRFirstnameTest'";

        // Add additional SQL commands as needed

    }
}
