using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FavTool.Models
{
    [Serializable]
    internal class HistoryCollectModel: ICollectModel
    {
	    [SerializeField] private List<string> _history = new List<string>();
	    internal List<string> History => _history;

	    [SerializeField] private int _maxHistoryCount = 25;

        internal bool IsDirty { get; set; }

        internal event Action onChangedHistory;

        public void Add(string guid)
        {
			_history.Add(guid);
            if (_history.Count > _maxHistoryCount)
                _history.RemoveAt(0);
			IsDirty = true;
            onChangedHistory?.Invoke();
        }

        public void Remove(string guid)
        {
	        _history.Remove(guid);
	        IsDirty = true;
            onChangedHistory?.Invoke();
		}

        internal void Clean()
        {
	        var indexes = new List<int>();
	        for (int i = History.Count - 1; i >= 0; i--)
		        if (String.IsNullOrEmpty(ToolUtils.GetPathByGuid(History[i])))
			        indexes.Add(i);

	        foreach (var index in indexes)
		        History.RemoveAt(index);
        }

		void ICollectModel.Clear()
        {
	        _history.Clear();
	        onChangedHistory?.Invoke();
        }

        public IEnumerable<IGrouping<string, string>> GetFrequency()
        {
	        return _history.GroupBy(x => x).OrderByDescending(x => x.Count());
        }
    }
}
