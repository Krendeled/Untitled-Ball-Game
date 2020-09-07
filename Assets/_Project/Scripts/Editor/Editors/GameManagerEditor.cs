using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UntitledBallGame.Editor.Editors
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            UIElementsEditorHelper.FillDefaultInspector(container, serializedObject, true);
            return container;
        }
    }
}