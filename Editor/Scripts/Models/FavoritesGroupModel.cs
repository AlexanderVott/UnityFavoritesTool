using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FavTool.Models
{
    [Serializable]
    internal class FavoritesGroupModel
    {
	    [SerializeField] protected string _key;
	    internal string key => _key;

        [SerializeField] protected Texture _icon;
        internal Texture icon => _icon;

        [SerializeField] protected List<string> _favoriteGUIDs = new List<string>();
        internal List<string> favoriteGUIDs => _favoriteGUIDs;

        internal bool IsEmpty => _favoriteGUIDs == null || _favoriteGUIDs.Count == 0;

        internal event Action<string> onAdded;
        internal event Action<string> onRemoved;

        internal FavoritesGroupModel(string keyParam, Texture iconParam)
        {
	        _key = keyParam;
	        _icon = iconParam;
        }
        internal FavoritesGroupModel(string keyParam, Texture iconParam, string[] guids)
        {
	        _key = keyParam;
	        _icon = iconParam;
            _favoriteGUIDs.AddRange(guids);
        }

        internal void Add(string guid)
        {
			_favoriteGUIDs.Add(guid);
			Sort();
			onAdded?.Invoke(guid);
        }

        internal void Remove(string guid)
        {
	        _favoriteGUIDs.Remove(guid);
	        Sort();
            onRemoved?.Invoke(guid);
        }

        internal void Sort()
        {
	        _favoriteGUIDs = _favoriteGUIDs.OrderBy(itr => Path.GetFileName(AssetDatabase.GUIDToAssetPath(itr))).ToList();
        }

        internal bool ContainsGuid(string guid)
        {
	        return _favoriteGUIDs.Contains(guid);
        }
    }
}
