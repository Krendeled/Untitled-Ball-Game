using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Malee.List;
using ReorderableList = Malee.List.ReorderableList;

namespace UntitledBallGame.Editor.Editors
{
    namespace UntitledBallGame.Editor
    {
        [CustomEditor(typeof(SceneProfile))]
        public class SceneProfileEditor : UnityEditor.Editor
        {
            private ReorderableList _reorderableList;
            
            public override VisualElement CreateInspectorGUI()
            {
                var container = new VisualElement();
                UIElementsEditorHelper.FillDefaultInspector(container, serializedObject, true);
                
                _reorderableList = new ReorderableList(serializedObject.FindProperty("_scenes"));
                
                _reorderableList.DoLayoutList();

                return container;
            }
        }
    }
}