using System.Collections.Generic;
using System.Linq;
using FavTool.Models;
using UnityEngine.UIElements;

namespace FavTool.SettingsPanels
{
	[ControllerTemplate("Settings/FavoritesFoldersSettings")]
    internal class FavoritesFoldersSettings : BaseController
    {
		private ScrollView _scroll;

		private VisualElement _createNewPanel;
		private Button _addNewFolderBtn;
		private Button _cancelNewFolderBtn;
		private TextField _textFieldNewFolder;

		private readonly List<FolderItemController> _items = new List<FolderItemController>();

	    internal FavoritesFoldersSettings(VisualElement root) : base(root, false)
	    {
		    _scroll = Panel.Q<ScrollView>("scroll");

		    InitVisualCreatePanel();
			Show();
	    }

	    internal override void UpdateView(bool isHide)
	    {
		    if (!isHide)
		    {
			    UpdateItems(_profile.Lists.lists);
		    }
	    }

	    private void Clear()
	    {
		    _scroll?.Clear();
		    _items?.Clear();
	    }

	    private void UpdateItems()
	    {
		    UpdateItems(_profile.Lists.lists);
		}

		private void UpdateItems(in List<SimpleCollectModel> list)
		{
			Clear();
			foreach (var itr in list)
				OnAddedItem(itr);
		}

		private void InitVisualCreatePanel()
		{
			_createNewPanel = _panel.Q<VisualElement>("createFolderPanel");
			_addNewFolderBtn = _createNewPanel.Q<Button>("createNewOk");
			_addNewFolderBtn.clicked += OnCreateNewFolder;
			_textFieldNewFolder = _createNewPanel.Q<TextField>("nameField");
		}

		private void OnCreateNewFolder()
		{
			var text = _textFieldNewFolder.value;
			_profile.Lists.Add(text);
		}

		protected override void SubscribeEvents()
	    {
		    base.SubscribeEvents();

		    _profile.Lists.onAdded += OnAddedItem;
		    _profile.Lists.onRemoved += OnRemovedItem;
		}

	    protected override void UnSubscribeEvents()
	    {
		    base.UnSubscribeEvents();
		    _profile.Lists.onAdded -= OnAddedItem;
		    _profile.Lists.onRemoved -= OnRemovedItem;
		}
		
	    private void OnAddedItem(SimpleCollectModel item)
	    {
		    _items.Insert(0, new FolderItemController(this, item));
			_scroll.Insert(0, _items.First().Visual);
	    }

	    private void OnRemovedItem(int index)
	    {
		    if (index >= 0 && index < _items.Count)
		    {
			    UpdateItems();
			}
		}

	    internal void Remove(SimpleCollectModel model)
	    {
		    if (_profile.Lists.lists.Count == 0)
			    return;
		    var index = _profile.Lists.lists.IndexOf(model);
			if (index >= 0)
				_profile.Lists.Remove(index);
	    }
    }
}
