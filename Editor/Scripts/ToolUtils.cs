using UnityEditor;
using UnityEngine;

namespace FavTool
{
    internal static class ToolUtils
    {
	    internal static Object GetPrefabParent(Object obj) => PrefabUtility.GetCorrespondingObjectFromSource(obj);

	    internal static string GetPathByGuid(string guid) => AssetDatabase.GUIDToAssetPath(guid);

	    internal static string GetGuidByPath(string path) => AssetDatabase.AssetPathToGUID(path);

	    internal static T GetObject<T>(string guid) where T: Object => AssetDatabase.LoadAssetAtPath<T>(GetPathByGuid(guid));

	    internal static Texture2D ToTexture2D(this Texture texture)
	    {
		    return Texture2D.CreateExternalTexture(
			    texture.width,
			    texture.height,
			    TextureFormat.RGB24,
			    false, false,
			    texture.GetNativeTexturePtr());
	    }

	    internal static Object GetAssetByGuid<T>(string guid) where T: Object => AssetDatabase.LoadAssetAtPath<T>(GetPathByGuid(guid));
		internal static Object GetAsset<T>(string path) where T : Object => AssetDatabase.LoadAssetAtPath<T>(path);

	    internal static Texture2D GetPreviewAsset(Object obj)
	    {
		    return AssetPreview.GetAssetPreview(obj);
	    }

	    internal static Texture2D GetThumbnailAsset(Object obj)
	    {
		    return AssetPreview.GetMiniThumbnail(obj);
	    }
    }
}
