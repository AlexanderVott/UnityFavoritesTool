using FavTool.Models;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class ItemVisual : VisualElement
    {
	    private FavoritesGroupModel data;
	    private string guid;

	    private ObjectField field;
		
	    public ItemVisual(FavoritesGroupModel item, string guid) : base()
	    {
			Initialize(item, guid);
	    }

	    private void Initialize(FavoritesGroupModel item, string guid)
	    {
		    this.data = item;
		    this.guid = guid;

		    var template = Resources.Load<VisualTreeAsset>("Item");
		    template.CloneTree(this);

		    field = this.Q<ObjectField>("groupItem");
		    field.allowSceneObjects = false;
		    field.objectType = typeof(Object);
		    field.value = ToolUtils.GetObject<Object>(this.guid);

		    var btnDelete = this.Q<Button>("btnDelete");
		    btnDelete.clickable.clicked += () =>
		    {
			    data.Remove(this.guid);
		    };
	    }
    }
}
