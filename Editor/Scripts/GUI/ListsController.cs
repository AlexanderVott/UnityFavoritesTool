using System;
using System.Collections.Generic;
using System.Linq;
using FavTool.GUI;
using FavTool.Models;
using UnityEditor;
using UnityEngine.UIElements;

namespace FavTool
{
	[ControllerTemplate("ListsControllerTemplate")]
	internal class ListsController : BaseController
    {
	    private ScrollView _listsScroll = null;
	    private ScrollView _headerScroll = null;

	    private Button lastHeaderBtn = null;

	    private readonly List<ItemController> _items = new List<ItemController>();

		internal ListsController(VisualElement root) : base(root)
		{
			InitHeader();
			InitScroll();
		}

		protected override void SubscribeEvents()
		{
			base.SubscribeEvents();
			_profile.Lists.onAdded += OnAddedHeaderItemList;
			_profile.Lists.onRemoved += onRemovedHeaderItemList;
			if (_listsScroll != null)
				_listsScroll.RegisterCallback<DragExitedEvent>(AddDraggable);
			_panel.parent.RegisterCallback<DragExitedEvent>(AddDraggable);
	    }

		protected override void UnSubscribeEvents()
		{
			base.UnSubscribeEvents();
			_panel.parent.UnregisterCallback<DragExitedEvent>(AddDraggable);
			if (_listsScroll != null)
				_listsScroll.UnregisterCallback<DragExitedEvent>(AddDraggable);
			_profile.Lists.onRemoved -= onRemovedHeaderItemList;
			_profile.Lists.onAdded -= OnAddedHeaderItemList;
		}
		internal override void Filter(string filterParam)
		{
			base.Filter(filterParam);
			if (lastHeaderBtn != null)
				UpdateList(lastHeaderBtn.userData as SimpleCollectModel);
		}

		private void OnAddedHeaderItemList(SimpleCollectModel model)
		{
			_headerScroll.Insert(0, new Button(null) { text = _profile.Lists.lists.First().name });
		}

		private void onRemovedHeaderItemList(int index)
		{
			_headerScroll.RemoveAt(index);
		}

		private void ClearHeader()
		{
			_headerScroll.Clear();
		}

		private void InitHeader()
		{
			_headerScroll = _panel.Q<ScrollView>("headerScroll");
		}

		private void InitScroll()
		{
			_listsScroll = _panel.Q<ScrollView>("listsScroll");
		}

		internal override void UpdateView(bool isHide)
		{
			base.UpdateView(isHide);
			if (isHide)
			{ 
			} 
			else
			{
				UpdateHeaderScroll();
				if (_profile.Lists.IsValidLastIndex())
					OnClickHeader(_profile.Lists.lists[_profile.Lists.LastIndex]);
			}
		}

		private void OnClickHeader(SimpleCollectModel model)
		{
			var btn = _headerScroll[_profile.Lists.lists.IndexOf(model)] as Button;
			if (btn == null)
			{
				return;
			}
			SwitchHeaderBtns(btn);
			UpdateList(model);
		}

		private void SwitchHeaderBtns(Button newBtn)
		{
			if (lastHeaderBtn != null)
			{
				lastHeaderBtn.RemoveFromClassList(UnityFavoriteTool.SelectedClassUSS);
				var collect = lastHeaderBtn.userData as SimpleCollectModel;
				if (collect != null)
					collect.onChangedCollect -= OnChangedCollect;
			}
			newBtn.AddToClassList(UnityFavoriteTool.SelectedClassUSS);

			var newCollect = newBtn.userData as SimpleCollectModel;
			if (newCollect != null)
				newCollect.onChangedCollect += OnChangedCollect;
			
			lastHeaderBtn = newBtn;
			_profile.Lists.UpdateLastIndex(newCollect);
		}

		private void OnChangedCollect()
		{
			UpdateList(lastHeaderBtn.userData as SimpleCollectModel);
		}

		private void ClearList()
		{
			_listsScroll.Clear();
			_items.Clear();
		}

		private void UpdateList(SimpleCollectModel model)
		{
			if (model == null)
				return;
			if (!IsActive)
				return;
			ClearList();

			var items = model.Items;
			var sortedHistory = String.IsNullOrEmpty(FilterValue)
				? items
				: ToolUtils.FilterGuids(items, FilterValue);
			var orderedHistory = sortedHistory.GroupBy(x => x)
				.OrderByDescending(x => x.Count());

			foreach (var itr in orderedHistory.Select(x => x.Key))
				AddItem(itr);
		}
		private void AddItem(string guid)
		{
			_items.Add(new ItemController(_profile.History, guid));
			var item = _items.Last();
			_listsScroll.Add(item.Visual);
		}

		private void UpdateHeaderScroll()
		{
			if (!IsActive)
				return;
			ClearHeader();
			foreach (var itr in _profile.Lists.lists)
				_headerScroll.Add(new Button(() => OnClickHeader(itr)) {text = itr.name, userData = itr});
		}

		private void AddDraggable(DragExitedEvent e)
		{
			if (lastHeaderBtn == null)
				return;

			var model = lastHeaderBtn.userData as SimpleCollectModel;
			foreach (var itr in DragAndDrop.paths)
				model.Add(ToolUtils.GetGuidByPath(itr));
		}
	}
}
