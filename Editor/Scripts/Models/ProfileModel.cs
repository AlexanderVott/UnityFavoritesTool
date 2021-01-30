using UnityEditor;
using UnityEngine;

namespace FavTool.Models
{
    public class ProfileModel : ScriptableObject
    {
	    private FavoritesModel m_favorites = new FavoritesModel();

	    internal void AddFavorite(Object obj)
	    {
			m_favorites.Add(obj);
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

	    public void CleanFavorites()
	    {
		    m_favorites.Clean();
		    SerializeFavorites();
	    }

	    private void SerializeFavorites()
	    {
			if (!m_favorites.IsDirty)
				return;

			EditorUtility.SetDirty(this);
			m_favorites.IsDirty = false;
		}
    }
}
