using FavTool.SettingsPanels;
using UnityEngine.UIElements;

namespace FavTool
{
    [ControllerTemplate("BaseControllerTemplate")]
    internal class SettingsController : BaseController
    {
	    private FavoritesFoldersSettings _folders = null;
	    private ScrollView _scroll = null;

		public override bool IsUseFilter { get; } = false;

	    internal SettingsController(VisualElement root) : base(root)
	    {
		    _scroll = Panel.Q<ScrollView>("scroll");
		    _folders = new FavoritesFoldersSettings(_scroll);
	    }

	    internal override void UpdateView(bool isHide)
	    {
		    base.UpdateView(isHide);
			_folders?.UpdateView(isHide);
	    }
    }
}