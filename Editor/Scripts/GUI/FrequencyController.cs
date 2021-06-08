using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace FavTool
{
    public class FrequencyController : BaseController
    {
	    private ScrollView _freqScroll;

	    private readonly List<ItemController> _items = new List<ItemController>();

	    internal FrequencyController(VisualElement root) : base(root)
	    {
		    _freqScroll = _panel.Q<ScrollView>("scroll");

			SubscribeEvents();
			OnChangedFrequency();
	    }

	    protected override void SubscribeEvents()
	    {
		    base.SubscribeEvents();
		    _profile.History.onChangedHistory += OnChangedFrequency;
	    }

	    protected override void UnSubscribeEvents()
	    {
		    base.UnSubscribeEvents();
		    _profile.History.onChangedHistory -= OnChangedFrequency;
	    }

		private void OnChangedFrequency()
		{
			if (!IsActive)
				return;

			Clear();

			var sortedHistory = String.IsNullOrEmpty(FilterValue) 
											? _profile.History.History 
											: ToolUtils.FilterGuids(_profile.History.History, FilterValue);
			var orderedHistory = sortedHistory.GroupBy(x => x)
								.OrderByDescending(x => x.Count());

			foreach (var itr in orderedHistory.Select(x => x.Key))
				AddItem(itr);
		}

		private void AddItem(string guid)
		{
			_items.Add(new ItemController(_profile.History, guid));
			var item = _items.Last();
			_freqScroll.Add(item.Visual);
		}

		internal override void Filter(string filterParam)
		{
			base.Filter(filterParam);
			OnChangedFrequency();
		}

		private void Clear()
		{
			_items.Clear();
			_freqScroll.Clear();
		}
    }
}
