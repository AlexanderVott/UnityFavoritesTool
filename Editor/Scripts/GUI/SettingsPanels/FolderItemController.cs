using FavTool.GUI;
using FavTool.Models;

namespace FavTool.SettingsPanels
{
    internal class FolderItemController
    {
	    private FolderItemVisual _visual;
	    internal FolderItemVisual Visual => _visual;

	    private string _name;
	    internal string Name => _name;
	    private FavoritesFoldersSettings _parent;
	    private SimpleCollectModel _model;

	    internal FolderItemController(FavoritesFoldersSettings parentSettings, SimpleCollectModel modelParameter)
	    {
			_parent = parentSettings;
			_model = modelParameter;
		    _name = modelParameter.name;

		    _visual = new FolderItemVisual(_name);

		    _visual.Field.value = _name;
		    _visual.BtnRename.clicked += () => _model.SetName(_visual.Field.value);
			_visual.BtnDelete.clicked += () => _parent.Remove(_model);
		}

	    internal void Destroy() => _visual.RemoveFromHierarchy();
	}
}