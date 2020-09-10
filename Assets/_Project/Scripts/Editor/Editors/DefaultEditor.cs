using UnityEditor;
using UnityEngine.UIElements;

namespace UntitledBallGame.Editor.Editors
{
    public class DefaultEditor<T> : UnityEditor.Editor
    {
        protected string GetLayoutPath => AssetHelper.GetPathFromLayoutName(typeof(T).Name);
        protected string GetStylePath => AssetHelper.GetPathFromStyleName(typeof(T).Name);
        
        protected VisualElement GetRoot()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetLayoutPath);
            var root = visualTree.CloneTree(); 
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(GetStylePath);
            root.styleSheets.Add(styleSheet);

            return root;
        }
    }
}