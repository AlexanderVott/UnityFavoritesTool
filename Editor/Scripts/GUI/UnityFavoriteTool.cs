using FavTool.Models;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
    public class UnityFavoriteTool : EditorWindow
    {
	    private ProfileModel _profile;
		private VisualElement _root;

		private BaseController _currentController;
		
		private readonly BaseController[] _controllers = new BaseController[4];
		private readonly VisualElement[] _tabs = new VisualElement[4];
		
		private string _filter;

		private const string SelectedClassUSS = "selected_tab";

		[MenuItem("Window/Show favorites window _%#T")]
	    public static void ShowFavoriteWindow()
	    {
		    var window = GetWindow<UnityFavoriteTool>();
		    window.titleContent = new GUIContent("Favorite Tool");
		    window.minSize = new Vector2(250, 50);
	    }

	    void OnEnable()
	    {
		    _profile = ProfileModel.Instance;
		    _root = rootVisualElement;
		    _root.styleSheets.Add(Resources.Load<StyleSheet>("WindowStyleSheet"));
			var windowTree = Resources.Load<VisualTreeAsset>("Window");
		    windowTree.CloneTree(_root);

		    PrepareWindow();
	    }

	    void OnDisable()
	    {
			foreach (var itr in _controllers)
				itr?.Dispose();
	    }

	    private void PrepareWindow()
	    {
			_controllers[0] = new FavoritesController(_root);
			_controllers[1] = new ListsController(_root);
			_controllers[2] = new HistoryController(_root);
			_controllers[3] = new FrequencyController(_root);

			var filterField = _root.Q<TextField>("filterField");
			filterField.RegisterCallback<ChangeEvent<string>>(x => _currentController?.Filter(x.newValue));
			
			_tabs[0] = _root.Q<VisualElement>("tab1");
			_tabs[1] = _root.Q<VisualElement>("tab2");
			_tabs[2] = _root.Q<VisualElement>("tab3");
			_tabs[3] = _root.Q<VisualElement>("tab4");
			_profile.onChangeState += SwitchTabs;
			_tabs[0].RegisterCallback<ClickEvent>(x => _profile.State = ProfileModel.ModeState.Favorites);
			_tabs[1].RegisterCallback<ClickEvent>(x => _profile.State = ProfileModel.ModeState.Lists);
			_tabs[2].RegisterCallback<ClickEvent>(x => _profile.State = ProfileModel.ModeState.History);
			_tabs[3].RegisterCallback<ClickEvent>(x => _profile.State = ProfileModel.ModeState.Frequency);
			
			SwitchTabs(_profile.State, ProfileModel.ModeState.Favorites);
	    }

	    private void SwitchTabs(ProfileModel.ModeState newState, ProfileModel.ModeState prevState)
	    {
		    _currentController?.Hide();

			_tabs[(int)prevState].RemoveFromClassList(SelectedClassUSS);
			_tabs[(int)newState].AddToClassList(SelectedClassUSS);
			_currentController = _controllers[(int) newState];
			
			if (_currentController != null)
		    {
			    _currentController.Show();
			    _currentController.Filter(_filter);
			}
	    }

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceId, int line)
		{
			ProfileModel.Instance.AddHistory(EditorUtility.InstanceIDToObject(instanceId));

			return false;
		}
    }
}
