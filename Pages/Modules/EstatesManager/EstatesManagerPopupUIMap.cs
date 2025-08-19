
using System.Data;

namespace TAF_iSAMS.Pages.Modules.EstatesManager
{
    /// <summary>
    /// Contains UI selectors specific to the "Create Form" popup window.
    /// </summary>
    public static class EstatesManagerPopupUIMap
    {
        public const string SaveButton = "#btnSave";
        public const string CloseButton = "#btnCancel";

        public const string BuildingNameLocator = "input[id='txtName']";
        public const string NewBuildingName = "TAF_Building";
        public const string BuildingInitialsLocator = "input[id='txtInitials']";
        public const string NewBuildingInitials = "TAFB";
        public const string BuildingDescriptionLocator = "input[id='txtDescription']";
        public const string HasClassroomsLocator = "input[id='intClassrooms']";
        public const string SchoolDivisionDropDown = "id='intDivision'";
        public const string SchoolDivisionDropDownOption = "All Divisions";
        public const string ClassroomNameLocator = "input[id='txtName']";
        public const string tempClassroomName = "TAFClassroom";
        public const string ClassroomInitialsLocator = "input[id='txtInitials']";
        public const string tempClassroomInitials = "TAFC";
        public const string ClassroomCapacityLocator = "input[id='intCapacity']";
        public const string tempClassroomCapacity = "10";
        public const string RoomTypesDropDown = "id='ddRoomTypes'";
        public const string RoomTypesDropDownOption = "Classroom type A";
        public const string Description = "input[id='txtDescription']";
        public const string ClassroomBuildingDropDown = "name='intBuilding'";
        public const string ClassroomBuildingDropDownOption = "Building A";

    }
}