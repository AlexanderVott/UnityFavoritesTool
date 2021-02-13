using FavTool.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    internal class ItemVisual : VisualElement
    {
	    private FavoritesGroupModel _data;
	    private string _guid;

	    private ObjectField _field;
	    private VisualElement _icon;
		
	    internal ItemVisual(FavoritesGroupModel item, string guid) : base()
	    {
			Initialize(item, guid);
	    }

	    private void Initialize(FavoritesGroupModel item, string guid)
	    {
		    this._data = item;
		    this._guid = guid;

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
		    _field.value = ToolUtils.GetObject<Object>(this._guid);
			_field.RegisterCallback<ChangeEvent<Object>>(e =>
			{
				if (e.newValue != e.previousValue && e.newValue != ToolUtils.GetObject<Object>(this._guid))
					_field.value = e.previousValue;
			});

		    var btnDelete = this.Q<Button>("btnDelete");
		    btnDelete.clickable.clicked += () =>
		    {
			    _data.Remove(this._guid);
		    };
	    }
    }
}
