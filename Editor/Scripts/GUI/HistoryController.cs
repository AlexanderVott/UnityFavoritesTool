using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace FavTool
{
	[ControllerTemplate("BaseControllerTemplate")]
	public class HistoryController : BaseController
    {
	    private ScrollView _historyScroll;

	    private readonly List<ItemController> _items = new List<ItemController>();

	    internal HistoryController(VisualElement root) : base(root)
	    {
		    _historyScroll = _panel.Q<ScrollView>("scroll");

			SubscribeEvents();
		    OnChangedCollect();
	    }

	    protected override void SubscribeEvents()
	    {
		    base.SubscribeEvents();
		    _profile.History.onChangedCollect += OnChangedCollect;
	    }

	    protected override void UnSubscribeEvents()
	    {
		    base.UnSubscribeEvents();
		    _profile.History.onChangedCollect -= OnChangedCollect;
		}

	    private void OnChangedCollect()
	    {
		    if (!IsActive)
			    return;

		    Clear();

		    var items = _profile.History.Items;
			var sortedHistory = String.IsNullOrEmpty(FilterValue) 
												? items 
												: ToolUtils.FilterGuids(items, FilterValue);

			sortedHistory.Reverse();
		    var groupedHistory = sortedHistory.GroupBy(x => x);

		    foreach (var itr in groupedHistory.Select(x => x.Key))
			    AddItem(itr);
		}

	    private void AddItem(string guid)
	    {
			_items.Add(new ItemController(_profile.History, guid));
			var item = _items.Last();
			_historyScroll.Add(item.Visual);
	    }

	    internal override void Filter(string filterParam)
	    {
		    base.Filter(filterParam);

			OnChangedCollect();
	    }

	    private void Clear()
	    {
		    _items.Clear();
		    _historyScroll.Clear();
	    }
    }
}
