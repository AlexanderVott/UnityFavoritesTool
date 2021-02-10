using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FavTool.Models
{
    [Serializable]
    public class FavoritesGroupModel
    {
	    [SerializeField] protected string m_key;
	    public string key => m_key;

        [SerializeField] protected Texture m_icon;
        public Texture icon => m_icon;

        [SerializeField] protected List<string> m_favoriteGUIDs = new List<string>();
        public List<string> favoriteGUIDs => m_favoriteGUIDs;

        public bool IsEmpty => m_favoriteGUIDs == null || m_favoriteGUIDs.Count == 0;

        internal event Action<string> onAdded;
        internal event Action<string> onRemoved;
        
        public FavoritesGroupModel(string keyParam, Texture iconParam)
        {
	        m_key = keyParam;
	        m_icon = iconParam;
        }
        public FavoritesGroupModel(string keyParam, Texture iconParam, string[] guids)
        {
	        m_key = keyParam;
	        m_icon = iconParam;
            m_favoriteGUIDs.AddRange(guids);
        }

        internal void Add(string guid)
        {
			m_favoriteGUIDs.Add(guid);
			Sort();
			onAdded?.Invoke(guid);
        }

        internal void Remove(string guid)
        {
	        m_favoriteGUIDs.Remove(guid);
	        Sort();
            onRemoved?.Invoke(guid);
        }

        internal void Sort()
        {
	        m_favoriteGUIDs = m_favoriteGUIDs.OrderBy(itr => Path.GetFileName(AssetDatabase.GUIDToAssetPath(itr))).ToList();
        }

        internal bool ContainsGuid(string guid)
        {
	        return m_favoriteGUIDs.Contains(guid);
        }
    }
}
