using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UI = TAF_iSAMS.Pages.Modules.TimetableManager.TimetableManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.TimetableManager
{
    internal class TimetableManagerSQL
    {

        // SQL commands for the Timetable Manager module
        public static string SqlDeleteTimetableDay = $"DELETE FROM TblTimetableManagerDays WHERE txtName = 'AUTO TIMETABLE DAY 1'";
        public static string SqlDeleteTimetableWeek = $"DELETE FROM TblTimetableManagerWeeks WHERE txtName = 'AUTO TIMETABLE WEEK 1'";
        public static string SqlDeleteEditedTimetableWeek = $"DELETE FROM TblTimetableManagerWeeks WHERE txtName = 'EDIT - AUTO TIMETABLE WEEK 1'";
        public static string SqlDeleteEditedTimetableDay = $"DELETE FROM TblTimetableManagerDays WHERE txtName = 'EDIT - AUTO TIMETABLE DAY 1'";
        public static string SqlDeleteTimetablePeriod = $"DELETE FROM TblTimetableManagerPeriods WHERE txtName = 'AUTO TIMETABLE PERIOD 1'";

        public static string SqlAddTimetableWeek = $"DECLARE @order int; SET @order = (SELECT TOP (1) intOrder FROM TblTimetableManagerWeeks ORDER BY intOrder DESC) + 1 INSERT INTO TblTimetableManagerWeeks (txtName, txtShortName, intOrder, intActive, txtSubmitBy, txtSubmitDateTime, blnShowTimetableWeek) VALUES ('AUTO TIMETABLE WEEK 1', 'A1', @order, 1, 'TAFUser', GETDATE(), 1)";
        public static string SqlAddTimetableDay = $"INSERT INTO TblTimetableManagerDays(intWeek, intDay, txtName, txtShortName, intOrder, intActive, txtSubmitBy, txtSubmitDateTime) VALUES((SELECT TOP(1) TblTimetableManagerWeeksID FROM TblTimetableManagerWeeks ORDER BY TblTimetableManagerWeeksID DESC), 2, 'AUTO TIMETABLE DAY 1', 'AUTO1', 1, 1, 'TAFUser', GETDATE())";

        

        // Add additional SQL commands as needed

    }
}
