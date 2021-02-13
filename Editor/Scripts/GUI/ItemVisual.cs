using FavTool.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    internal class ItemVisual : VisualElement
    {
		private ObjectField _field;
	    public ObjectField Field => _field;

	    private VisualElement _icon;
	    public VisualElement Icon => _icon;

	    private Button _btnDelete;
	    public Button BtnDelete => _btnDelete;
		
	    internal ItemVisual(FavoritesGroupModel item, string guid) => Initialize(item, guid);

	    private void Initialize(FavoritesGroupModel item, string guid)
	    {
		    var template = Resources.Load<VisualTreeAsset>("Item");
		    template.CloneTree(this);

		    _icon = this.Q<VisualElement>("iconItem");
		    var preview = ToolUtils.GetPreviewAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    if (preview == null)
		    {
			    preview = ToolUtils.GetThumbnailAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    }
		    var bg = Background.FromTexture2D(preview);
			_icon.style.backgroundImage = bg;

		    _field = this.Q<ObjectField>("groupItem");
		    _field.allowSceneObjects = false;
		    _field.objectType = typeof(Object);
		    
		    _btnDelete = this.Q<Button>("btnDelete");
		    
	    }
    }
}
