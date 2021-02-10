using FavTool.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class MainWindow : EditorWindow
    {
	    private ProfileModel m_profile;
		private ScrollView m_groupsScroll;
		private VisualElement m_root;
		private VisualElement m_graggablePanel;

		[MenuItem("Window/Show favorites window _%#T")]
	    public static void ShowFavoriteWindow()
	    {
		    var window = GetWindow<MainWindow>();
		    window.titleContent = new GUIContent("RedDev Favorite Tool");
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

	    void OnDisable()
	    {
		    UnSubscribeEvents();
	    }

	    private void PrepareWindow()
	    {
		    m_groupsScroll = m_root.Q<ScrollView>("groupsScroll");

		    m_graggablePanel = m_root;//.Q<VisualElement>("graggablePanel");

		    foreach (var itrG in m_profile.Favorites.groups)
		    {
				OnAddedGroup(itrG);
		    }
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
		    m_groupsScroll.Add(new GroupVisual(group));
	    }

	    private void AddDraggable(DragExitedEvent e)
	    {
		    foreach (var itr in DragAndDrop.paths) 
			    m_profile.AddFavorite(ToolUtils.GetGuidByPath(itr), itr);
	    }
    }
}
