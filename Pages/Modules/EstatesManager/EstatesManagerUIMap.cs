using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;
using PUI = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerPopupUIMap;


namespace TAF_iSAMS.Pages.Modules.EstatesManager
{
    internal class EstatesManagerUIMap
    {
        public static string ModuleContentLocator = "#module-content-" + ModuleSelectorHelper.GetModuleNr("School Manager");
        public static string ModuleContentFrame = "txtFrameContent";

        public const string CreateSchoolBuildingIconSRC = "src='/Legacy/system/images/16/folder_add.gif'";
        public const string CreateSchoolBuildingButtonText = "Create School Building";
        public const string CreateSchoolBuildingButton = "span:has-text('Create School Building')";
        public const string CreateSchoolClassroomButton = "span:has-text('Create School Classroom')";

        public const string CreatedBuildingOnPage = $"a:has-text({PUI.NewBuildingName})";
        public const string BuildingASelector = "a:has-text('Building A')";
        public const string CreatedClassroomOnPage = $"a:has-text(\"{PUI.tempClassroomName}\")";
    }
}
