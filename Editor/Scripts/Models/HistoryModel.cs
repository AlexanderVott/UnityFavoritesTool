using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FavTool
{
    [Serializable]
    internal class HistoryModel
    {
	    [SerializeField] private List<string> _history = new List<string>();
	    internal List<string> History => _history;

	    [SerializeField] private int _maxHistoryCount = 25;

        internal bool IsDirty { get; set; }

        internal event Action onChangedHistory;

        internal void Add(string guid)
        {
			_history.Add(guid);
            if (_history.Count > _maxHistoryCount)
                _history.RemoveAt(0);
			IsDirty = true;
            onChangedHistory?.Invoke();
        }

        internal void Remove(string guid)
        {
	        _history.Remove(guid);
	        IsDirty = true;
            onChangedHistory?.Invoke();
        }

        internal void Clear()
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
