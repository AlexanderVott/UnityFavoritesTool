using FavTool.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class UnityFavoriteTool : EditorWindow
    {
	    private ProfileModel profile;
		private VisualElement root;

		private BaseController currentController;
		
		private BaseController[] controllers = new BaseController[3];
		private VisualElement[] tabs = new VisualElement[3];
		
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
		    profile = ProfileModel.Instance;
		    root = rootVisualElement;
		    root.styleSheets.Add(Resources.Load<StyleSheet>("WindowStyleSheet"));
			var windowTree = Resources.Load<VisualTreeAsset>("Window");
		    windowTree.CloneTree(root);

		    PrepareWindow();
	    }

	    void OnDisable()
	    {
			foreach (var itr in controllers)
				itr?.Dispose();
	    }

	    private void PrepareWindow()
	    {
			controllers[0] = new FavoritesController(root);
			controllers[1] = new HistoryController(root);
			controllers[2] = new FrequencyController(root);

			var filterField = root.Q<TextField>("filterField");
			filterField.RegisterCallback<ChangeEvent<string>>(x => currentController?.Filter(x.newValue));
			
			tabs[0] = root.Q<VisualElement>("tab1");
			tabs[1] = root.Q<VisualElement>("tab2");
			tabs[2] = root.Q<VisualElement>("tab3");
			profile.onChangeState += SwitchTabs;
			tabs[0].RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.Favorites);
			tabs[1].RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.History);
			tabs[2].RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.Frequency);
			
			SwitchTabs(profile.State, ProfileModel.ModeState.Favorites);
	    }

	    private void SwitchTabs(ProfileModel.ModeState newState, ProfileModel.ModeState prevState)
	    {
		    currentController?.Hide();

			tabs[(int)prevState].RemoveFromClassList(SelectedClassUSS);
			tabs[(int)newState].AddToClassList(SelectedClassUSS);
			currentController = controllers[(int) newState];
			
			if (currentController != null)
		    {
			    currentController.Filter(filter);
			    currentController.Show();
			}
	    }
    }
}
