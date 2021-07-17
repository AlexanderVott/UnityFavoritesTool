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

		internal ItemVisual(string guid) => Initialize(guid);

	    private void Initialize(string guid)
	    {
		    var template = Resources.Load<VisualTreeAsset>("Item");
		    template.CloneTree(this);

		    _icon = this.Q<VisualElement>("iconItem");

		    _field = this.Q<ObjectField>("groupItem");
		    _field.allowSceneObjects = false;
		    _field.objectType = typeof(Object);
		    
		    _btnDelete = this.Q<Button>("btnDelete");
	    }

	    internal void SetIcon(Texture2D texture)
	    {
		    _icon.style.backgroundImage = Background.FromTexture2D(texture);
	    }
    }
}
