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
    internal class FavoritesModel
    {
        [SerializeField] private List<FavoritesGroupModel> _groups = new List<FavoritesGroupModel>();
        internal List<FavoritesGroupModel> groups => _groups;

		internal bool IsDirty { get; set; }

		internal event Action<FavoritesGroupModel> onAddedGroup;
		internal event Action<FavoritesGroupModel> onRemovedGroup;
		internal event Action<FavoritesGroupModel> onChangedGroups;

		internal void Add(Object obj)
		{
			var path = ToolUtils.GetPath(obj);
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
				var group = new FavoritesGroupModel(typeName, icon, new[] {guid});
				_groups.Add(group);
				Sort();
				IsDirty = true;
				onAddedGroup?.Invoke(group);
			}
			else
			{
				var group = _groups.Find(itr => itr.key == typeName);
				if (!group.ContainsGuid(guid))
				{
					group.Add(guid);
					IsDirty = true;
				}

				onChangedGroups?.Invoke(group);
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
					onChangedGroups?.Invoke(itr);
					return;
				}
			}
		}

		internal void Toggle(Object obj)
		{
			var path = ToolUtils.GetPath(obj);
			var guid = ToolUtils.GetGuidByPath(path);

			if (Contains(guid))
				Remove(guid);
			else
				Add(guid, path);
		}

		internal bool ContainsGroup(string typeName) => _groups.Exists(itr => itr.key == typeName);
		
		internal bool Contains(string guid) => _groups.Any(@group => @group.ContainsGuid(guid));

		internal void Sort()
		{
			_groups = _groups.OrderByDescending(itr => itr.key).ToList();
		}

		internal void Clean()
		{
			var markedToRemoveGroups = new List<FavoritesGroupModel>();
			foreach (var itr in _groups)
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
				if (markedToRemoveGUIDs.Count > 0)
					onChangedGroups?.Invoke(itr);

				if (itr.IsEmpty)
					markedToRemoveGroups.Add(itr);
			}

			foreach (var itr in markedToRemoveGroups)
			{
				_groups.Remove(itr);
				onRemovedGroup?.Invoke(itr);
			}

			IsDirty = true;
		}
    }
}