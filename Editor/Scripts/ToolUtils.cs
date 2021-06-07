using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FavTool
{
    internal static class ToolUtils
    {
	    internal static Object GetPrefabParent(Object obj) => PrefabUtility.GetCorrespondingObjectFromSource(obj);

	    internal static string GetPathByGuid(string guid) => AssetDatabase.GUIDToAssetPath(guid);

	    internal static string GetGuidByPath(string path) => AssetDatabase.AssetPathToGUID(path);
		
	    internal static string GetPath(Object obj)
	    {
		    if (obj == null)
		    {
			    return String.Empty;
		    }
		    var prefabRef = ToolUtils.GetPrefabParent(obj);
		    return AssetDatabase.GetAssetPath(prefabRef != null ? prefabRef : obj);
	    }

		internal static Texture2D ToTexture2D(this Texture texture)
	    {
		    return Texture2D.CreateExternalTexture(
			    texture.width,
			    texture.height,
			    TextureFormat.RGB24,
			    false, false,
			    texture.GetNativeTexturePtr());
	    }

	    internal static T GetAssetByGuid<T>(string guid) where T: Object => AssetDatabase.LoadAssetAtPath<T>(GetPathByGuid(guid));
		
	    internal static Object GetAsset<T>(string path) where T : Object => AssetDatabase.LoadAssetAtPath<T>(path);

	    internal static Texture2D GetPreviewAsset(Object obj)
	    {
		    return AssetPreview.GetAssetPreview(obj);
	    }

	    internal static Texture2D GetThumbnailAsset(Object obj)
	    {
		    return AssetPreview.GetMiniThumbnail(obj);
	    }

	    internal static List<string> FilterGuids(in List<string> guids, in string filterValue)
	    {
		    var result = new List<string>();
		    var lowerfilterValue = filterValue.ToLower();
		    foreach (var itr in guids)
		    {
			    var name = Path.GetFileNameWithoutExtension(ToolUtils.GetPathByGuid(itr));
			    if (!name.ToLower().Contains(lowerfilterValue))
				    continue;
			    result.Add(itr);
		    }

		    return result;
	    }
	}
}
