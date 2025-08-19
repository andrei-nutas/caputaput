using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.TimetableManager
{
    internal class TimetableManagerUIMap
    {

        // UI element selectors for the Timetable Manager page

        #region Timetable Manager
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Timetable Manager");
        #endregion

        public static string ModuleContentFrame = "txtFrameContent";
        public static string ManagePeriodsAndDaysButton = "a:has-text('Manage Periods & Days')";
        public static string CreateADayButton = "a:has-text('Create a Day')";
        public static string CreateADayAppears = "a:has-text('AUTO TIMETABLE DAY 1')";
        public static string EditedDayAppears = "a:has-text('EDIT - AUTO TIMETABLE DAY 1')";
        public static string CreateAWeekAppears = "a:has-text('AUTO TIMETABLE WEEK 1')";
        public static string EditedWeekAppears = "a:has-text('EDIT - AUTO TIMETABLE WEEK 1')";
        public static string CreateAWeekButton = "a:has-text('Create a Week')";
        public static string CreateAPeriodButton = "a:has-text('Create a Period')";
        public static string ClickTimetableWeekInTree = "a:has-text('Week 1 [WK1]')";
        public static string ClickTimetableDayInTree = "a:has-text('Monday [Mon]')";
        public static string CreateAPeriodAppears = "a:has-text('AUTO TIMETABLE PERIOD 1')";
        public static string EditAWeekButton = "a:has-text('Edit Week Properties')";
        public static string EditADayButton = "a:has-text('Edit Day Properties')";


        // Add additional UI element selectors as needed.

    }
}
