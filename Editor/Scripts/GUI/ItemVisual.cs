using FavTool.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    internal class ItemVisual : VisualElement
    {
	    private FavoritesGroupModel data;
	    private string guid;

	    private ObjectField field;
	    private VisualElement icon;
		
	    internal ItemVisual(FavoritesGroupModel item, string guid) : base()
	    {
			Initialize(item, guid);
	    }

	    private void Initialize(FavoritesGroupModel item, string guid)
	    {
		    this.data = item;
		    this.guid = guid;

		    var template = Resources.Load<VisualTreeAsset>("Item");
		    template.CloneTree(this);

		    icon = this.Q<VisualElement>("iconItem");
		    var preview = ToolUtils.GetPreviewAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    if (preview == null)
		    {
			    preview = ToolUtils.GetThumbnailAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    }
		    var bg = Background.FromTexture2D(preview);
			icon.style.backgroundImage = bg;

		    field = this.Q<ObjectField>("groupItem");
		    field.allowSceneObjects = false;
		    field.objectType = typeof(Object);
		    field.value = ToolUtils.GetObject<Object>(this.guid);
			field.RegisterCallback<ChangeEvent<Object>>(e =>
			{
				if (e.newValue != e.previousValue && e.newValue != ToolUtils.GetObject<Object>(this.guid))
					field.value = e.previousValue;
			});

		    var btnDelete = this.Q<Button>("btnDelete");
		    btnDelete.clickable.clicked += () =>
		    {
			    data.Remove(this.guid);
		    };
	    }
    }
}
