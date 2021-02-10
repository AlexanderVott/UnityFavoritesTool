using System;
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

		private readonly string SelectedClassUSS = "selected_tab";

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
		    m_root.styleSheets.Add(Resources.Load<StyleSheet>("WindowStyleSheet"));
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

			var tab1 = m_root.Q<VisualElement>("tab1");
			var tab2 = m_root.Q<VisualElement>("tab2");
			var tab3 = m_root.Q<VisualElement>("tab3");
			m_profile.onChangeState += (newValue, prefValue) =>
			{
				m_groupsScroll.AddToClassList("hide");

				tab1.RemoveFromClassList(SelectedClassUSS);
				tab2.RemoveFromClassList(SelectedClassUSS);
				tab3.RemoveFromClassList(SelectedClassUSS);
				switch (newValue)
				{
					case ProfileModel.ModeState.Favorites:
						m_groupsScroll.RemoveFromClassList("hide");
						tab1.AddToClassList(SelectedClassUSS);
						break;
					case ProfileModel.ModeState.History:
						tab2.AddToClassList(SelectedClassUSS);
						break;
					case ProfileModel.ModeState.Frequency:
						tab3.AddToClassList(SelectedClassUSS);
						break;
				}
			};
			tab1.RegisterCallback<ClickEvent>(x => m_profile.State = ProfileModel.ModeState.Favorites);
			tab2.RegisterCallback<ClickEvent>(x => m_profile.State = ProfileModel.ModeState.History);
			tab3.RegisterCallback<ClickEvent>(x => m_profile.State = ProfileModel.ModeState.Frequency);

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
					return;
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
