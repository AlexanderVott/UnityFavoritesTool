using System.Collections.Generic;
using FavTool.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
	internal class GroupVisual : VisualElement
    {
	    private FavoritesGroupModel _data;

	    private VisualElement _groupContent;

	    private readonly Dictionary<string, ItemVisual> _items = new Dictionary<string, ItemVisual>();

	    internal GroupVisual(FavoritesGroupModel group) : base()
	    {
		    Initialize(group, null);
		    SubscribeEvents();
	    }

	    internal GroupVisual(FavoritesGroupModel group, in List<string> guids) : base()
	    {
		    Initialize(group, guids);
		    SubscribeEvents();
	    }

		private void Initialize(FavoritesGroupModel group, in List<string> guids)
	    {
		    const string UEngineName = "UnityEngine.";

		    this._data = group;
		    var template = Resources.Load<VisualTreeAsset>("GroupItem");
		    template.CloneTree(this);

		    var foldout = this.Q<Foldout>("groupFoldout");
		    foldout.text = @group.key.Contains(UEngineName) 
							? @group.key.Remove(0, UEngineName.Length) 
							: @group.key;

		    var texture = this.Q<VisualElement>("iconGroup");
		    var bg = Background.FromTexture2D(@group.icon.ToTexture2D());
		    texture.style.backgroundImage = bg;

		    _groupContent = this.Q<VisualElement>("groupContent");

		    InitializeItems(guids != null ? guids : group.favoriteGUIDs);
	    }

		private void InitializeItems(in List<string> guids)
		{
			foreach (var itr in guids)
				OnAddedItem(itr);
		}

		internal void SubscribeEvents()
	    {
		    _data.onAdded += OnAddedItem;
		    _data.onRemoved += OnRemovedItem;
	    }

	    internal void UnsubscribeEvents()
	    {
		    _data.onAdded -= OnAddedItem;
		    _data.onRemoved -= OnRemovedItem;
		    _items.Clear();
	    }

	    private void OnAddedItem(string guid)
	    {
		    _items.Add(guid, new ItemVisual(_data, guid));
			_groupContent.Add(_items[guid]);
	    }

	    private void OnRemovedItem(string guid)
	    {
			_items[guid].RemoveFromHierarchy();
			_items.Remove(guid);
			ProfileModel.Instance.CleanFavorites();
			Clean();
	    }

		internal void Clean()
		{
			if (_groupContent.childCount == 0) 
				RemoveFromHierarchy();
		}
	}
}
