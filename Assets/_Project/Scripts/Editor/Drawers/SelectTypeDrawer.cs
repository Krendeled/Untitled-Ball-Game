using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    [CustomPropertyDrawer(typeof(SelectTypeAttribute))]
    public class SelectTypeDrawer : PropertyDrawer
    {
        private Type[] _types;
        private SerializedProperty _serializedTypeProperty;
        private string[] _displayedTypes;
        private Type _selectedType;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 58;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _serializedTypeProperty = property.FindPropertyRelative("_serializedType");

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(position, label);
                DrawSplitter(position);
                
                if (_types == null || DrawButton(position))
                {
                    RefreshTypes();
                    RefreshDisplayedTypes();
                    _selectedType = GetSerializedType();
                }
                
                if (_types.Contains(_selectedType) == false)
                    _serializedTypeProperty.stringValue = ClassTypeReference.NoneElement;
                
                int i = DrawPopup(position);
                if (i == 0)
                    _serializedTypeProperty.stringValue = ClassTypeReference.NoneElement;
                else
                    _serializedTypeProperty.stringValue = _types[i - 1].AssemblyQualifiedName;
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

        private int DrawPopup(Rect position)
        {
            var popupRect = new Rect(position.x + 6, position.y + 32, position.width - 38, 20);

            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            
            return EditorGUI.Popup(popupRect, GetSelectedIndex(), _displayedTypes, style);
        }

        private Type GetSerializedType()
        {
            if (string.IsNullOrEmpty(_serializedTypeProperty.stringValue)) return null;
            return CachedTypes.GetType(_serializedTypeProperty.stringValue);
        }
        
        private int GetSelectedIndex()
        {
            if (_selectedType == null) return 0;
            
            return Array.IndexOf(_types, _selectedType) + 1;
        }

        private void RefreshTypes()
        {
            if (attribute is SelectTypeAttribute implAttribute)
            {
                _types = ReflectionUtility.GetSubtypes(implAttribute.FieldType,
                    t => !t.IsAbstract && !t.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
            }
        }

        private void RefreshDisplayedTypes()
        {
            _displayedTypes = _types.Select(t => t.FullName).Prepend(ClassTypeReference.NoneElement).ToArray();
            
            if (attribute is SelectTypeAttribute selectTypeAttr)
            {
                switch (selectTypeAttr.Representation)
                {
                    case Representation.Name:
                        _displayedTypes = _types.Select(t => t.Name).Prepend(ClassTypeReference.NoneElement).ToArray();
                        break;
                    case Representation.FullName:
                        _displayedTypes = _types.Select(t => t.FullName).Prepend(ClassTypeReference.NoneElement).ToArray();
                        break;
                }
            }
        }
    }
}