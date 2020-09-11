using UnityEditor;
using UnityEngine.UIElements;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Editors
{
    [CustomEditor(typeof(SceneProfileManager))]
    public class SceneProfileManagerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            UIElementsEditorHelper.FillDefaultInspector(container, serializedObject, true);
            return container;
        }
    }
}