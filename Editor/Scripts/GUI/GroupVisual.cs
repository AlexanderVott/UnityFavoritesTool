using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
	internal class GroupVisual : VisualElement
	{
		private Foldout _titleFoldout;
		public Foldout TitleFoldout => _titleFoldout;

		private VisualElement _groupContent;
	    public VisualElement GroupContent => _groupContent;

	    private VisualElement _background;

		internal GroupVisual() : base()
	    {
		    Initialize();
	    }

	    private void Initialize()
	    {
		    var template = Resources.Load<VisualTreeAsset>("GroupItem");
		    template.CloneTree(this);

		    _titleFoldout = this.Q<Foldout>("groupFoldout");

		    _background = this.Q<VisualElement>("iconGroup");
		    
		    _groupContent = this.Q<VisualElement>("groupContent");
	    }

		internal void SetBackground(Texture2D texture) => 
			_background.style.backgroundImage = Background.FromTexture2D(texture);
	}
}
