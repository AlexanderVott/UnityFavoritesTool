using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FavTool.Models
{
	public class ProfileModel : ScriptableObject
	{
		internal enum ModeState
		{
			Favorites = 0,
			Lists = 1,
			History,
			Frequency,
			Settings,
		}

		[SerializeField] private ModeState _state = ModeState.Favorites;
		internal ModeState State
		{
			get => _state;
			set
			{
				var oldState = _state;
				_state = value;
				onChangeState?.Invoke(_state, oldState);
			}
		}

		internal event Action<ModeState, ModeState> onChangeState;

		[SerializeField] private FavoritesModel _favorites = new FavoritesModel();
		internal FavoritesModel Favorites => _favorites;

		[SerializeField] private SimpleCollectModel _history = new SimpleCollectModel("history");
		internal SimpleCollectModel History => _history;

		[SerializeField] private FavoritesListsModel _lists = new FavoritesListsModel();
		internal FavoritesListsModel Lists => _lists;

		[SerializeField] private bool autoSaveProfileAsset = false;

		private static ProfileModel _instance;
		internal static ProfileModel Instance
		{
			get
			{
				if (!_instance)
					_instance = Initialize();

				return _instance;
			}
		}

		private static ProfileModel Initialize()
		{
			var config = AssetDatabase.FindAssets("ProfileModel t:ProfileModel", null);
			ProfileModel profile = null;
			if (config.Length >= 1)
			{
				profile = ToolUtils.GetAssetByGuid<ProfileModel>(config[0]);
			}
			if (profile == null)
			{
				return CreateNewProfile();
			}

			profile.CleanFavorites();
			profile.CleanHistory();

			return profile;
		}

		private static ProfileModel CreateNewProfile()
		{
			var result = ScriptableObject.CreateInstance<ProfileModel>();

			var directory = GetDirectoryProfile();

			var finalAssetName = directory + "/ProfileModel.asset";
			string path = AssetDatabase.GenerateUniqueAssetPath(finalAssetName);

			AssetDatabase.CreateAsset(result, path);
			AssetDatabase.SaveAssets();

			return result;
		}

		private static string GetDirectoryProfile()
		{
			var directories = Directory.GetDirectories(Application.dataPath, "Editor", SearchOption.AllDirectories);
			var packageDir = directories.FirstOrDefault(itr => itr.Contains("UnityFavoriteTool-package"));
			if (String.IsNullOrEmpty(packageDir))
			{
				packageDir = Path.Combine(Application.dataPath, "FavoriteTool", "Editor", "Resources");
			}

			if (!Directory.Exists(packageDir))
				Directory.CreateDirectory(packageDir);
			packageDir = "Assets/" + packageDir.Substring(Application.dataPath.Length);

			if (String.IsNullOrEmpty(packageDir))
			{
				Debug.LogError($"Not found directory package");
				return null;
			}

			return packageDir;
		}

		internal void AddHistory(Object obj)
		{
			var path = ToolUtils.GetPath(obj);
			var guid = ToolUtils.GetGuidByPath(path);
			_history.Add(guid);
			CleanHistory();
		}

		internal void RemoveHistory(string guid)
		{
			_history.Remove(guid);
			CleanHistory();
		}

		internal void AddFavorite(Object obj)
		{
			_favorites.Add(obj);
			SerializeFavorites();
		}

		internal void AddFavorite(string guid, string path)
		{
			_favorites.Add(guid, path);
			SerializeFavorites();
		}

		internal void RemoveFavorite(string guid)
		{
			_favorites.Remove(guid);
			SerializeFavorites();
		}

		internal void ToggleFavorite(Object obj)
		{
			_favorites.Toggle(obj);
			SerializeFavorites();
		}

		internal bool ContainsFavorite(string guid) => _favorites.Contains(guid);

		internal void CleanFavorites()
		{
			_favorites.Clean();
			SerializeFavorites();
		}

		internal void CleanHistory()
		{
			_history.Clean();
			SerializeFavorites();
		}

		private void SerializeFavorites()
		{
			if (!_favorites.IsDirty)
				return;

			EditorUtility.SetDirty(this);
			if (autoSaveProfileAsset)
				AssetDatabase.SaveAssets();
			_favorites.IsDirty = false;
		}
	}
}
