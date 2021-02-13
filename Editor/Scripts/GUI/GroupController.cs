using System.Collections.Generic;
using FavTool.GUI;
using FavTool.Models;

namespace FavTool
{
    internal class GroupController
	{
		private GroupVisual _visual;
		internal GroupVisual Visual => _visual;

		private FavoritesGroupModel _data;

		private readonly Dictionary<string, ItemController> _items = new Dictionary<string, ItemController>();

		internal GroupController(FavoritesGroupModel group): this(group, null)
		{
		}

		internal GroupController(FavoritesGroupModel group, in List<string> guids)
		{
			const string UEngineName = "UnityEngine.";

			this._data = group;

			_visual = new GroupVisual();
			_visual.TitleFoldout.text = group.key.Contains(UEngineName)
				? group.key.Remove(0, UEngineName.Length)
				: group.key;

			_visual.SetBackground(_data.icon.ToTexture2D());

			InitializeItems(guids != null ? guids : group.favoriteGUIDs);

			SubscribeEvents();
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
			_items.Add(guid, new ItemController(_data, guid));
			_visual.GroupContent.Add(_items[guid].Visual);
		}

		private void OnRemovedItem(string guid)
		{
			_items[guid].Destroy();
			_items.Remove(guid);
			ProfileModel.Instance.CleanFavorites();
			Clean();
		}
		
		internal void Clean()
		{
			if (_visual.GroupContent.childCount == 0)
				Destroy();
		}

		internal void Destroy() => _visual.RemoveFromHierarchy();
	}
}
