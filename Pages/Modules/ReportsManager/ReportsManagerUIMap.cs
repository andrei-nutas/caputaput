using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;

namespace TAF_iSAMS.Pages.Modules.ReportsManager
{
    internal class ReportsManagerUIMap
    {
        public const string AddReportsManagerButton = "#addReportsManagerButton";
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("Reports Manager");
        public static string ModuleContentFrame = "iFrameContent";
        public static string ModuleOptionsFrame = "iFrameOptions";
    }
}
