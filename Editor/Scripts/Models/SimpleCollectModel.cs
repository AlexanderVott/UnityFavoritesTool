using System;
using System.Collections.Generic;
using UnityEngine;

namespace FavTool.Models
{
    [Serializable]
    internal class SimpleCollectModel: ICollectModel
    {
	    [SerializeField] private string _name;
	    public string name => _name;

        [SerializeField] private List<string> _items = new List<string>();
	    internal List<string> Items => _items;

	    [SerializeField] private int _maxItemsCount = 25;

        internal bool IsDirty { get; set; }

        internal event Action onChangedCollect;

        internal SimpleCollectModel(string nameParameter)
        {
	        _name = nameParameter;
        }

        internal void SetName(string nameParameter)
        {
	        _name = nameParameter;
        }

        public void Add(string guid)
        {
			_items.Add(guid);
            if (_items.Count > _maxItemsCount)
                _items.RemoveAt(0);
			IsDirty = true;
            onChangedCollect?.Invoke();
        }

        public void Remove(string guid)
        {
	        _items.Remove(guid);
	        IsDirty = true;
            onChangedCollect?.Invoke();
		}

        internal void Clean()
        {
	        var indexes = new List<int>();
	        for (int i = Items.Count - 1; i >= 0; i--)
		        if (String.IsNullOrEmpty(ToolUtils.GetPathByGuid(Items[i])))
			        indexes.Add(i);

	        foreach (var index in indexes)
		        Items.RemoveAt(index);
        }

		void ICollectModel.Clear()
        {
	        _items.Clear();
	        onChangedCollect?.Invoke();
        }
    }
}
