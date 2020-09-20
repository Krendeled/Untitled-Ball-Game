using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;

namespace UntitledBallGame.Editor.Drawers.IMGUI
{
    [CustomPropertyDrawer(typeof(SerializeReferencePickerAttribute))]
    public class SerializeReferencePickerDrawer : PropertyDrawer
    {
        private Type _baseType;
        private Type _selectedType;
        private Type[] _implementations;
        private string[] _displayedTypes;
        private bool _propertyVisible;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float inspectorHeight = 58;
            if (_propertyVisible && property.isExpanded)
                return inspectorHeight + EditorGUI.GetPropertyHeight(property, true);
            if (_propertyVisible)
                return inspectorHeight + EditorGUIUtility.singleLineHeight;
            return inspectorHeight;
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(position, label);
                DrawSplitter(position);
                
                if (!string.IsNullOrEmpty(property.managedReferenceFullTypename))
                {
                    DrawPropertyField(position, property);
                    _propertyVisible = true;
                }
                else
                {
                    _propertyVisible = false;
                }
                
                if (_baseType == null)
                {
                    _baseType = property.GetManagedReferenceFieldType();
                }
                
                if (_implementations == null || DrawButton(position))
                {
                    RefreshImplementations();
                    RefreshDisplayedTypes();
                }

                _selectedType = property.GetManagedReferenceFullType();
                
                if (_implementations.Contains(_selectedType) == false)
                    property.managedReferenceValue = null;
                
                int i = DrawPopup(position, property);
                if (i == 0)
                    property.managedReferenceValue = null;
                else if (_implementations[i - 1] != _selectedType)
                    property.managedReferenceValue = Activator.CreateInstance(_implementations[i - 1]);
            }
        }
    
        private void DrawBackground(Rect position)
        {
            var color = new Color(0f, 0f, 0f, 0.2f);
            EditorGUI.DrawRect(position, color);
        }
    
        private void DrawTitle(Rect position, GUIContent label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter,
                padding = new RectOffset(0, 0, 4, 4)
            };
    
            EditorGUI.LabelField(position, label, style);
        }
    
        private void DrawSplitter(Rect position)
        {
            var rect = new Rect(position.x + 2, position.y + 22, position.width - 4, 2);
            var color = new Color(255f, 255f, 255f, 0.1f);
            EditorGUI.DrawRect(rect, color);
        }
    
        private bool DrawButton(Rect position)
        {
            var rect = new Rect(position.x + position.width - 26, position.y + 32, 20, 20);
            var icon = EditorGUIUtility.IconContent("d_Refresh");
            var style = new GUIStyle(GUI.skin.button) 
            {
                padding = new RectOffset(0, 0, 0, 0), 
                margin = new RectOffset(0, 0, 0, 0)
            };
            return GUI.Button(rect, icon, style);
        }
    
        private int DrawPopup(Rect position, SerializedProperty property)
        {
            var popupRect = new Rect(position.x + 6, position.y + 32, position.width - 38, 20);

            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            
            return EditorGUI.Popup(popupRect, GetSelectedIndex(), _displayedTypes, style);
        }

        private void DrawPropertyField(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel++;

            var rect = new Rect(position.x, position.y + 52, position.width - 6, 16);
            EditorGUI.PropertyField(rect, property, true);

            EditorGUI.indentLevel--;
        }

        private int GetSelectedIndex()
        {
            if (_selectedType == null) return 0;
            
            return Array.IndexOf(_implementations, _selectedType) + 1;
        }

        private void RefreshImplementations()
        {
            _implementations = ReflectionUtility.GetSubtypes(_baseType, 
                t => !t.IsAbstract && !t.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
    
        private void RefreshDisplayedTypes()
        {
            if (attribute is SerializeReferencePickerAttribute pickerAttr)
            {
                switch (pickerAttr.Representation)
                {
                    case Representation.Name:
                        _displayedTypes = _implementations.Select(t => t.Name).Prepend("(None)").ToArray();
                        break;
                    case Representation.FullName:
                        _displayedTypes = _implementations.Select(t => t.FullName).Prepend("(None)").ToArray();
                        break;
                }
            }
        }
    }
}