using UnityEditor;
using UnityEngine.UIElements;

namespace UntitledBallGame.Editor.Drawers
{
    public abstract class DefaultPropertyDrawer<T> : PropertyDrawer
    {
        protected readonly string _uxmlPath = AssetHelper.GetPathFromLayoutName(typeof(T).Name);
        protected readonly string _ussPath = AssetHelper.GetPathFromStyleName(typeof(T).Name);

        protected VisualElement GetRoot()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_uxmlPath);
            var root = visualTree.CloneTree(); 
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(_ussPath);
            root.styleSheets.Add(styleSheet);

            return root;
        }
    }
}