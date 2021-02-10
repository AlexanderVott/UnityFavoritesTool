using System.Collections.Generic;
using FavTool.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class GroupVisual : VisualElement
    {
	    private FavoritesGroupModel data;

	    private VisualElement groupContent;

	    private Dictionary<string, ItemVisual> items = new Dictionary<string, ItemVisual>();
		
	    public GroupVisual(FavoritesGroupModel group) : base()
	    {
		    Initialize(group, null);
		    SubscribeEvents();
	    }

	    public GroupVisual(FavoritesGroupModel group, in List<string> guids) : base()
	    {
		    Initialize(group, guids);
		    SubscribeEvents();
	    }

		private void Initialize(FavoritesGroupModel group, in List<string> guids)
	    {
		    const string UEngineName = "UnityEngine.";

		    this.data = group;
		    var template = Resources.Load<VisualTreeAsset>("GroupItem");
		    template.CloneTree(this);

		    var foldout = this.Q<Foldout>("groupFoldout");
		    foldout.text = @group.key.Contains(UEngineName) 
							? @group.key.Remove(0, UEngineName.Length) 
							: @group.key;

		    var texture = this.Q<VisualElement>("iconGroup");
		    var bg = Background.FromTexture2D(@group.icon.ToTexture2D());
		    texture.style.backgroundImage = bg;

		    groupContent = this.Q<VisualElement>("groupContent");

		    InitializeItems(guids != null ? guids : group.favoriteGUIDs);
	    }

		private void InitializeItems(in List<string> guids)
		{
			foreach (var itr in guids)
				OnAddedItem(itr);
		}

	    public void SubscribeEvents()
	    {
		    data.onAdded += OnAddedItem;
		    data.onRemoved += OnRemovedItem;
	    }

	    public void UnsubscribeEvents()
	    {
		    data.onAdded -= OnAddedItem;
		    data.onRemoved -= OnRemovedItem;
		    items.Clear();
	    }

	    private void OnAddedItem(string guid)
	    {
		    items.Add(guid, new ItemVisual(data, guid));
			groupContent.Add(items[guid]);
	    }

	    private void OnRemovedItem(string guid)
	    {
			items[guid].RemoveFromHierarchy();
			items.Remove(guid);
			ProfileModel.Instance.CleanFavorites();
			Clean();
	    }

		internal void Clean()
		{
			if (groupContent.childCount == 0)
			{
				RemoveFromHierarchy();
			}
		}
	}
}
