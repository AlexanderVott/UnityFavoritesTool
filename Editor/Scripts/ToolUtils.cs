using UnityEditor;
using UnityEngine;

namespace FavTool
{
    internal static class ToolUtils
    {
	    internal static Object GetPrefabParent(Object obj) => PrefabUtility.GetCorrespondingObjectFromSource(obj);

	    internal static string GetPathByGUID(string guid) => AssetDatabase.GUIDToAssetPath(guid);

	    internal static string GetGUIDByPath(string path) => AssetDatabase.AssetPathToGUID(path);
    }
}
