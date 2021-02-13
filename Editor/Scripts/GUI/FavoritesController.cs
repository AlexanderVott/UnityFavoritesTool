using System.Collections.Generic;
using System.IO;
using System.Linq;
using FavTool.GUI;
using FavTool.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace FavTool
{
    internal class FavoritesController : BaseController
    {
	    private ProfileModel profile;
	    private ScrollView favGroupsScroll;

		internal FavoritesController(VisualElement root) : base(root)
		{
			profile = ProfileModel.Instance;
			favGroupsScroll = panel.Q<ScrollView>("favGroupsScroll");

			foreach (var itrG in profile.Favorites.groups)
				OnAddedGroup(itrG);

			SubscribeEvents();
		}

		private void SubscribeEvents()
		{
			profile.Favorites.onAddedGroup += OnAddedGroup;
			panel.RegisterCallback<DragExitedEvent>(AddDraggable);
		}

		private void UnSubscribeEvents()
		{
			panel.UnregisterCallback<DragExitedEvent>(AddDraggable);
			profile.Favorites.onAddedGroup -= OnAddedGroup;

			foreach (var itr in favGroupsScroll.Children())
				if (itr is GroupVisual group)
					group.UnsubscribeEvents();
		}

		private void OnAddedGroup(FavoritesGroupModel group)
	    {
		    if (!string.IsNullOrEmpty(FilterValue))
		    {
			    var filteredItems = FilterGuids(@group.favoriteGUIDs);
			    if (filteredItems.Count == 0)
				    return;
			    favGroupsScroll.Add(new GroupVisual(@group, filteredItems));
		    }
		    else
			    favGroupsScroll.Add(new GroupVisual(@group));
		}

		private void AddDraggable(DragExitedEvent e)
		{
			if (!string.IsNullOrEmpty(FilterValue))
			{
				foreach (var itr in DragAndDrop.paths)
					profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
			}
			else
			{
				foreach (var itr in DragAndDrop.paths)
					if (Path.GetFileNameWithoutExtension(itr).Contains(FilterValue))
						profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
			}
		}

		internal void ClearVisual()
		{
			favGroupsScroll.Clear();
		}

		internal void CleanGroups()
		{
			foreach (var itr in favGroupsScroll.Children().Select(x => x as GroupVisual))
			{
				if (itr == null)
					continue;
				itr.Clean();
			}
		}
		private List<string> FilterGuids(List<string> guids)
		{
			var result = new List<string>();
			foreach (var itr in guids)
			{
				var name = Path.GetFileNameWithoutExtension(ToolUtils.GetPathByGuid(itr));
				if (!name.Contains(FilterValue))
					continue;
				result.Add(itr);
			}

			return result;
		}

		internal override void Filter(string filterParam)
		{
			FilterValue = filterParam;
			ClearVisual();
			foreach (var itrG in profile.Favorites.groups)
				OnAddedGroup(itrG);
			CleanGroups();
		}

		public override void Dispose()
		{
			UnSubscribeEvents();
		}
    }
}
