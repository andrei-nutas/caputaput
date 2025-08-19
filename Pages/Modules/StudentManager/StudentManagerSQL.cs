using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI = TAF_iSAMS.Pages.Modules.StudentManager.StudentManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.StudentManager
{
    internal class StudentManagerSQL
    {

        // SQL commands for the Student Manager module

        public static string SqlDeleteTestStudent = $"DELETE FROM TblPupilManagementPupils WHERE txtFullName = 'AutomationTest AddPupil'";
        public static string SqlAddSearchCurrentPupil = "INSERT INTO TblPupilManagementPupils(txtSchoolID, txtSchoolCode, txtOfficialName, txtTitle, txtForename, txtSurname, txtInitials, txtPreName, txtFullName, txtLabelSalutation, txtLetterSalutation, txtGender, txtType, txtEnquiryDate, txtAdmissionsStatus, txtProspectusEnquiryDate, txtSubmitBy, txtSubmitDateTime, intSystemStatus, intAutoSchoolCodeNumericPart) SELECT(convert(numeric(12,0),rand() * 899999999999) + 100000000000),'PRE0000000TST','TEST,AUTOMATIONSEARCHCURRENT', 'Mr', 'TEST','AUTOMATIONSEARCHCURRENT','T','TESTAUTO','TESTAUTOMATIONSEARCHCURRENT','TESTAUTOMATIONSEARCHCURRENT','TESTAUTOMATIONSEARCHCURRENT','F','Day',(GETDATE()),'Enquiry',(GETDATE()),'TEST', (GETDATE()),1,'9999'";
        public static string SqlDeleteSearchCurrentPupil = "DELETE FROM TblPupilManagementPupils WHERE txtFullName = 'TESTAUTOMATIONSEARCHCURRENT' AND intSystemStatus = '1'";

        public static string SqlAddSearchApplicant = "INSERT INTO TblPupilManagementPupils(txtSchoolID, txtSchoolCode, txtOfficialName, txtForename, txtSurname, txtInitials, txtPreName, txtFullName, txtLabelSalutation, txtLetterSalutation, txtGender, txtType, txtEnquiryDate, txtAdmissionsStatus, txtProspectusEnquiryDate, txtSubmitBy, txtSubmitDateTime, intSystemStatus, intAutoSchoolCodeNumericPart) SELECT(convert(numeric(12,0),rand() * 899999999999) + 100000000000),'PRE0000000TST','TEST,AUTOMATIONSEARCH','TEST','AUTOMATIONSEARCH','T','TESTAUTO','TEST AUTOMATIONSEARCH','TEST AUTOMATIONSEARCH','TEST AUTOMATIONSEARCH','F','Day',(GETDATE()),'Enquiry',(GETDATE()),'TEST', (GETDATE()),0,'9999'";
        public static string SqlDeleteSearchApplicant = "DELETE FROM TblPupilManagementPupils WHERE txtFullName = 'TEST AUTOMATIONSEARCH'";

        public static string SqlAddSearchFormerPupil = "INSERT INTO TblPupilManagementPupils(txtSchoolID, txtSchoolCode, txtOfficialName, txtTitle, txtForename, txtSurname, txtInitials, txtPreName, txtFullName, txtLabelSalutation, txtLetterSalutation, txtGender, txtType, txtEnquiryDate, txtAdmissionsStatus, txtProspectusEnquiryDate, txtSubmitBy, txtSubmitDateTime, intSystemStatus, intAutoSchoolCodeNumericPart) SELECT(convert(numeric(12,0),rand() * 899999999999) + 100000000000),'PRE0000000TST','TEST,AUTOMATIONSEARCH', 'Mr', 'TEST','AUTOMATIONSEARCH','T','TESTAUTO','TEST AUTOMATIONSEARCH','TEST AUTOMATIONSEARCH','TEST AUTOMATIONSEARCH','F','Day',(GETDATE()),'Enquiry',(GETDATE()),'TEST', (GETDATE()),-1,'9999'";
        public static string SqlDeleteSearchFormerPupil = "DELETE FROM TblPupilManagementPupils WHERE txtFullName = 'TEST AUTOMATIONSEARCH' AND intSystemStatus = '-1'";


        // Add additional SQL commands as needed

    }
}
