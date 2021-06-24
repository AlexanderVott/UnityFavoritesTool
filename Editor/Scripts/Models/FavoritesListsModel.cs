using System;
using System.Collections.Generic;
using UnityEngine;

namespace FavTool.Models
{
    [Serializable]
    internal class FavoritesListsModel
    {
		[SerializeField] private List<SimpleCollectModel> _lists = new List<SimpleCollectModel>();
	    internal List<SimpleCollectModel> lists => _lists;

	    internal int LastIndex = -1;

	    internal bool IsDirty { get; set; } = false;

        internal event Action<SimpleCollectModel> onAdded;
        internal event Action<int> onRemoved;
        internal event Action<SimpleCollectModel> onChanged;

        internal void Add(string nameParameter)
        {
	        if (ContainsItem(nameParameter))
		        return;

	        var model = new SimpleCollectModel(nameParameter);
            _lists.Insert(0, model);

            onAdded?.Invoke(model);

            IsDirty = true;
        }

        internal void Remove(int index)
        {
	        if (index < 0 && index >= _lists.Count)
		        return;

	        _lists.RemoveAt(index);

            onRemoved?.Invoke(index);

            IsDirty = true;
        }

        internal bool ContainsItem(string nameParameter)
        {
	        return _lists.Exists(x => x.name == nameParameter);
        }

        internal void UpdateLastIndex(SimpleCollectModel model)
        {
	        LastIndex = _lists.IndexOf(model);
        }

        internal bool IsValidLastIndex()
        {
	        return LastIndex > -1 && LastIndex < _lists.Count;
        }
    }
}
