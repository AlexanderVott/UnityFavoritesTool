using UnityEditor;
using UnityEngine;

namespace FavTool
{
    internal static class ToolUtils
    {
	    internal static Object GetPrefabParent(Object obj) => PrefabUtility.GetCorrespondingObjectFromSource(obj);

	    internal static string GetPathByGuid(string guid) => AssetDatabase.GUIDToAssetPath(guid);

	    internal static string GetGuidByPath(string path) => AssetDatabase.AssetPathToGUID(path);
    }
}
