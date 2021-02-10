using System.Collections.Generic;
using System.IO;
using System.Linq;
using FavTool.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class UnityFavoriteTool : EditorWindow
    {
	    private ProfileModel m_profile;
		private ScrollView m_groupsScroll;
		private VisualElement m_root;
		private VisualElement m_graggablePanel;

		private string filter;
		
		[MenuItem("Window/Show favorites window _%#T")]
	    public static void ShowFavoriteWindow()
	    {
		    var window = GetWindow<UnityFavoriteTool>();
		    window.titleContent = new GUIContent("Favorite Tool");
		    window.minSize = new Vector2(250, 50);
	    }

	    void OnEnable()
	    {
		    m_profile = ProfileModel.Instance;
		    m_root = rootVisualElement;
		    var windowTree = Resources.Load<VisualTreeAsset>("Window");
		    windowTree.CloneTree(m_root);
			
			PrepareWindow();
			SubscribeEvents();
	    }

	    void OnDisable() => UnSubscribeEvents();

	    private void PrepareWindow()
	    {
		    m_groupsScroll = m_root.Q<ScrollView>("groupsScroll");

		    m_graggablePanel = m_root;

		    var filterField = m_root.Q<TextField>("filterField");
			filterField.RegisterCallback<ChangeEvent<string>>(x =>
			{
				Filter(x.newValue);
			});


			foreach (var itrG in m_profile.Favorites.groups) 
			    OnAddedGroup(itrG);
	    }

	    private void SubscribeEvents()
	    {
		    m_graggablePanel.RegisterCallback<DragExitedEvent>(AddDraggable);
			m_profile.Favorites.onAddedGroup += OnAddedGroup;
	    }

	    private void UnSubscribeEvents()
	    {
			m_graggablePanel.UnregisterCallback<DragExitedEvent>(AddDraggable);
		    m_profile.Favorites.onAddedGroup -= OnAddedGroup;

		    foreach (var itr in m_groupsScroll.Children())
			    if (itr is GroupVisual group)
				    group.UnsubscribeEvents();
	    }

	    private void OnAddedGroup(FavoritesGroupModel group)
	    {
		    if (!string.IsNullOrEmpty(filter))
		    {
			    var filteredItems = FilterGuids(@group.favoriteGUIDs);
				if (filteredItems.Count == 0)
				{
					return;
				}
				m_groupsScroll.Add(new GroupVisual(@group, filteredItems));
		    } else 
				m_groupsScroll.Add(new GroupVisual(@group));
	    }

		private List<string> FilterGuids(List<string> guids)
		{
			var result = new List<string>();
			foreach (var itr in guids)
			{
				var name = Path.GetFileNameWithoutExtension(ToolUtils.GetPathByGuid(itr));
				if (!name.Contains(filter))
					continue;
				result.Add(itr);
			}

			return result;
		}

	    private void AddDraggable(DragExitedEvent e)
	    {
		    if (!string.IsNullOrEmpty(filter))
		    {
			    foreach (var itr in DragAndDrop.paths)
				    m_profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
		    }
		    else
		    {
			    foreach (var itr in DragAndDrop.paths)
					if (Path.GetFileNameWithoutExtension(itr).Contains(filter))
						m_profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
			}
	    }

	    internal void ClearVisual()
	    {
		    m_groupsScroll.Clear();
	    }

	    internal void CleanGroups()
	    {
		    foreach (var itr in m_groupsScroll.Children().Select(x=>x as GroupVisual))
		    {
				if (itr == null)
					continue;
				itr.Clean();
		    }
	    }

	    internal void Filter(string filterValue)
	    {
		    filter = filterValue;
		    ClearVisual();
		    foreach (var itrG in m_profile.Favorites.groups) 
			    OnAddedGroup(itrG);
		    CleanGroups();
	    }
    }
}
