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

		private VisualElement favoritesPanel;
		private VisualElement historyPanel;
		private VisualElement frequencyPanel;

		private BaseController currentController;

		private FavoritesController favController;
		private HistoryController histController;
		private FrequencyController freqController;

		private VisualElement tab1;
		private VisualElement tab2;
		private VisualElement tab3;

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
		    favController.Dispose();
		    histController.Dispose();
		    freqController.Dispose();
	    }

	    private void PrepareWindow()
	    {
		    favoritesPanel = root.Q<VisualElement>("favoritesPanel");
		    historyPanel   = root.Q<VisualElement>("historyPanel");
		    frequencyPanel = root.Q<VisualElement>("frequencyPanel");

		    favController = new FavoritesController(favoritesPanel);
		    histController = new HistoryController(historyPanel);
		    freqController = new FrequencyController(frequencyPanel);

			var filterField = root.Q<TextField>("filterField");
			filterField.RegisterCallback<ChangeEvent<string>>(x => currentController?.Filter(x.newValue));

			tab1 = root.Q<VisualElement>("tab1");
			tab2 = root.Q<VisualElement>("tab2");
			tab3 = root.Q<VisualElement>("tab3");
			profile.onChangeState += (newValue, prefValue) => SwitchTabs(newValue);
			tab1.RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.Favorites);
			tab2.RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.History);
			tab3.RegisterCallback<ClickEvent>(x => profile.State = ProfileModel.ModeState.Frequency);
			
			SwitchTabs(profile.State);
	    }

	    private void HidePanel(VisualElement panel) => panel.AddToClassList("hide");
		private void ShowPanel(VisualElement panel) => panel.RemoveFromClassList("hide");

	    private void SwitchTabs(ProfileModel.ModeState state)
	    {
		    HidePanel(favoritesPanel);
		    HidePanel(historyPanel);
		    HidePanel(frequencyPanel);

		    tab1.RemoveFromClassList(SelectedClassUSS);
		    tab2.RemoveFromClassList(SelectedClassUSS);
		    tab3.RemoveFromClassList(SelectedClassUSS);
		    currentController = null;
		    switch (state)
		    {
			    case ProfileModel.ModeState.Favorites:
				    ShowPanel(favoritesPanel);
				    tab1.AddToClassList(SelectedClassUSS);
				    currentController = favController;
				    break;
			    case ProfileModel.ModeState.History:
				    ShowPanel(historyPanel);
				    tab2.AddToClassList(SelectedClassUSS);
				    currentController = histController;
				    break;
			    case ProfileModel.ModeState.Frequency:
				    ShowPanel(frequencyPanel);
				    tab3.AddToClassList(SelectedClassUSS);
				    currentController = freqController;
				    break;
		    }
			currentController?.Filter(filter);
	    }
    }
}
