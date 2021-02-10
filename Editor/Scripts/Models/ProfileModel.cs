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
	    [SerializeField] private FavoritesModel m_favorites = new FavoritesModel();
	    internal FavoritesModel Favorites => m_favorites;

	    private static ProfileModel m_instance;
		internal static ProfileModel Instance
	    {
		    get
		    {
			    if (!m_instance)
			    {
				    m_instance = Initialize();
			    }

			    return m_instance;
		    }
	    }

	    private static ProfileModel Initialize()
	    {
		    var config = AssetDatabase.FindAssets("ProfileModel t:ProfileModel", null);
		    ProfileModel profile = null;
		    if (config.Length >= 1)
		    {
			    profile = ToolUtils.GetObject<ProfileModel>(config[0]);
		    }
			if (profile == null)
			{
				return CreateNewProfile();
			}

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
		    packageDir = "Assets/" + packageDir.Substring(Application.dataPath.Length );

			if (String.IsNullOrEmpty(packageDir))
		    {
				Debug.LogError($"Not found directory package");
				return null;
		    }

		    return packageDir;
	    }

	    internal void AddFavorite(Object obj)
	    {
			m_favorites.Add(obj);
			SerializeFavorites();
	    }

		internal void AddFavorite(string guid, string path)
		{
			m_favorites.Add(guid, path);
			SerializeFavorites();
		}

	    internal void RemoveFavorite(string guid)
	    {
			m_favorites.Remove(guid);
			SerializeFavorites();
	    }

	    internal void ToggleFavorite(Object obj)
	    {
			m_favorites.Toggle(obj);
			SerializeFavorites();
	    }

	    internal bool ContainsFavorite(string guid) => m_favorites.Contains(guid);

	    internal void CleanFavorites()
	    {
		    m_favorites.Clean();
		    SerializeFavorites();
	    }

	    private void SerializeFavorites()
	    {
			if (!m_favorites.IsDirty)
				return;

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			m_favorites.IsDirty = false;
		}
    }
}
