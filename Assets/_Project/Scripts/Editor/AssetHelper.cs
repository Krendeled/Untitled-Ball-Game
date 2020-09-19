using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UntitledBallGame.Editor
{
    public static class AssetHelper
    {
        public static string GetPathFromLayoutName(string name)
        {
            return GetPathFromTypeAndName("VisualTreeAsset", name);
        }
        
        public static string GetPathFromStyleName(string name)
        {
            return GetPathFromTypeAndName("StyleSheet", name);
        }

        private static string GetPathFromTypeAndName(string type, string name)
        {
            var assets = AssetDatabase.FindAssets($"t:{type} {name}");

            if (assets.Length == 0)
            {
                Debug.LogWarning($"Asset {name} of type {type} was not found.");
                return string.Empty;
            }
            if (assets.Length > 1)
            {
                Debug.LogWarning($"Multiple assets with name {name} of type {type} were found.");
                return string.Empty;
            }
            
            return AssetDatabase.GUIDToAssetPath(assets[0]);
        }
    }
}