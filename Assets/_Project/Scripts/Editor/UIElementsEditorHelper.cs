using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UntitledBallGame.Editor
{
    public static class UIElementsEditorHelper
    {
        public static void FillDefaultInspector(VisualElement container, SerializedObject serializedObject, bool hideScript)
        {
            SerializedProperty property = serializedObject.GetIterator();
            if (property.NextVisible(true)) // Expand first child.
            {
                do
                {
                    if (property.propertyPath == "m_Script" && hideScript)
                    {
                        continue;
                    }

                    var field = new PropertyField(property) {name = "PropertyField:" + property.propertyPath};


                    if (property.propertyPath == "m_Script" && serializedObject.targetObject != null)
                    {
                        field.SetEnabled(false);
                    }
 
                    container.Add(field);
                }
                while (property.NextVisible(false));
            }
        }
    }
}