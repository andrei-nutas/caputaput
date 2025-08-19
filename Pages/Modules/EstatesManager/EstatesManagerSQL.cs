using PUI = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerPopupUIMap;
using UI = TAF_iSAMS.Pages.Modules.EstatesManager.EstatesManagerUIMap;


namespace TAF_iSAMS.Pages.Modules.EstatesManager
{
    internal class EstatesManagerSQL
    {
        public const string RemoveCreatedBuilding = $"DELETE FROM TblSchoolManagementBuildings WHERE txtName = '{PUI.NewBuildingName}'";
        public const string RemoveCreatedClassroom = $"DELETE FROM TblSchoolManagementClassrooms WHERE txtName = '{PUI.tempClassroomName}'";
    }
}
