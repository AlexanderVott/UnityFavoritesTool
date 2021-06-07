using System.Collections.Generic;
using System.IO;
using System.Linq;
using FavTool.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace FavTool
{
    internal class FavoritesController : BaseController
    {
	    private ScrollView _favGroupsScroll;
	    private readonly List<GroupController> _groups = new List<GroupController>();

		internal FavoritesController(VisualElement root) : base(root)
		{
			_favGroupsScroll = _panel.Q<ScrollView>("favGroupsScroll");

			foreach (var itrG in _profile.Favorites.groups)
				OnAddedGroup(itrG);

			SubscribeEvents();
		}

		protected override void SubscribeEvents()
		{
			_profile.Favorites.onAddedGroup += OnAddedGroup;
			_panel.RegisterCallback<DragExitedEvent>(AddDraggable);
		}

		protected override void UnSubscribeEvents()
		{
			_panel.UnregisterCallback<DragExitedEvent>(AddDraggable);
			_profile.Favorites.onAddedGroup -= OnAddedGroup;

			foreach (var itr in _groups)
				itr.UnsubscribeEvents();
		}

		private void OnAddedGroup(FavoritesGroupCollectModel group)
	    {
		    if (!string.IsNullOrEmpty(FilterValue))
		    {
			    var filteredItems = ToolUtils.FilterGuids(@group.favoriteGUIDs, FilterValue);
			    if (filteredItems.Count == 0)
				    return;
			    _groups.Add(new GroupController(@group, filteredItems));
			    _favGroupsScroll.Add(_groups.Last().Visual);
		    }
		    else
		    {
				_groups.Add(new GroupController(@group));
			    _favGroupsScroll.Add(_groups.Last().Visual);
		    }
	    }

		private void AddDraggable(DragExitedEvent e)
		{
			if (string.IsNullOrEmpty(FilterValue))
			{
				foreach (var itr in DragAndDrop.paths)
					_profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
			}
			else
			{
				foreach (var itr in DragAndDrop.paths)
					if (Path.GetFileNameWithoutExtension(itr).Contains(FilterValue))
						_profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
			}
		}

		internal void ClearVisual()
		{
			_groups.Clear();
			_favGroupsScroll.Clear();
		}

		internal void CleanGroups()
		{
			foreach (var itr in _groups)
			{
				if (itr == null)
					continue;
				itr.Clean();
			}
		}
		
		internal override void Filter(string filterParam)
		{
			FilterValue = filterParam;
			ClearVisual();
			foreach (var itrG in _profile.Favorites.groups)
				OnAddedGroup(itrG);
			CleanGroups();
		}
    }
}
