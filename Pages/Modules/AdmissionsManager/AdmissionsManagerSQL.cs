using Azure.Core;
using System;
using TAF_iSAMS.Pages.Utilities;
using Ui = TAF_iSAMS.Pages.Modules.AdmissionsManager.AdmissionsManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.AdmissionsManager

{

    internal class AdmissionsManagerSQL

    {

        // SQL commands for the Admissions Manager module


        public static string SqlDeleteApplicant = $"DELETE FROM TblPupilManagementPupils WHERE txtForename = '{Ui.ApplicantForename}'";
        public static string SqlAddSearchApplicant = "INSERT INTO TblPupilManagementPupils(txtSchoolID, txtSchoolCode, txtOfficialName, txtForename, txtSurname, txtInitials, txtPreName, txtFullName, txtLabelSalutation, txtLetterSalutation, txtGender, txtType, txtEnquiryDate, txtAdmissionsStatus, txtProspectusEnquiryDate, txtSubmitBy, txtSubmitDateTime, intSystemStatus, intAutoSchoolCodeNumericPart) SELECT(convert(numeric(12,0),rand() * 899999999999) + 100000000000),'PRE0000000TST','TEST,AUTOMATIONSEARCH','TEST','AUTOMATIONSEARCH','T','TESTAUTO','TESTAUTOMATIONSEARCH','TESTAUTOMATIONSEARCH','TESTAUTOMATIONSEARCH','F','Day',(GETDATE()),'Enquiry',(GETDATE()),'TEST', (GETDATE()),0,'9999'";
        public static string SqlDeleteSearchApplicant = "DELETE FROM TblPupilManagementPupils WHERE txtFullName = 'TESTAUTOMATIONSEARCH'";


        // Add additional SQL commands as needed

    }

}

