using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace FavTool.Models
{
    [Serializable]
    public class FavoritesModel
    {
        [SerializeField] private List<FavoritesGroupModel> m_groups = new List<FavoritesGroupModel>();
        public List<FavoritesGroupModel> groups => m_groups;

		internal bool IsDirty { get; set; }

		internal void Add(Object obj)
		{
			var path = GetPath(obj);
			string guid = ToolUtils.GetGuidByPath(path);

			Add(guid, path);
		}

		internal void Add(string guid, string path)
		{
			if (Directory.Exists(path))
			{
				return;
			}
			var type = AssetDatabase.GetMainAssetTypeAtPath(path);
			Assert.IsNotNull(type, $"Not found type by path {path}");

			string typeName = type.FullName;
			Texture icon = EditorGUIUtility.ObjectContent(null, type).image;

			if (!ContainsGroup(typeName))
			{
				m_groups.Add(new FavoritesGroupModel(typeName, icon, new []{ guid }));
				Sort();
			}
			else
			{
				var group = m_groups.Find(itr => itr.key == typeName);
				if (!group.ContainsGuid(guid))
					group.Add(guid);
			}

			IsDirty = true;
		}

		internal void Remove(string guid)
		{
			foreach (var itr in groups)
			{
				if (itr.ContainsGuid(guid))
				{
					itr.Remove(guid);
					IsDirty = true;
					return;
				}
			}
		}

		internal void Toggle(Object obj)
		{
			var path = GetPath(obj);
			var guid = ToolUtils.GetGuidByPath(path);

			if (Contains(guid))
				Remove(guid);
			else
				Add(guid, path);
		}

		private string GetPath(Object obj)
		{
			var prefabRef = ToolUtils.GetPrefabParent(obj);
			return AssetDatabase.GetAssetPath(prefabRef != null ? prefabRef : obj);
		}

		internal bool ContainsGroup(string typeName) => m_groups.Exists(itr => itr.key == typeName);
		
		internal bool Contains(string guid) => m_groups.Any(@group => @group.ContainsGuid(guid));

		internal void Sort()
		{
			m_groups = m_groups.OrderByDescending(itr => itr.key).ToList();
		}

		internal void Clean()
		{
			var markedToRemoveGroups = new List<FavoritesGroupModel>();
			foreach (var itr in m_groups)
			{
				var markedToRemoveGUIDs = new List<string>();
				foreach (var guid in itr.favoriteGUIDs)
				{
					string path = ToolUtils.GetPathByGuid(guid);

					if (String.IsNullOrEmpty(path) 
							|| (!File.Exists(path)))//TODO: directory check
						markedToRemoveGUIDs.Add(guid);
				}

				foreach (var guid in markedToRemoveGUIDs)
					itr.favoriteGUIDs.Remove(guid);

				if (itr.IsEmpty)
					markedToRemoveGroups.Add(itr);
			}

			foreach (var itr in markedToRemoveGroups)
				markedToRemoveGroups.Remove(itr);
		}
    }
}