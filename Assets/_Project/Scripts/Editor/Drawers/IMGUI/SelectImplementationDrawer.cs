using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;

namespace UntitledBallGame.Editor.Drawers.IMGUI
{
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    [CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
    public class SelectImplementationDrawer : PropertyDrawer
    {
        private Type[] _implementations;
        private SerializedProperty _serializedTypeProperty;
        private string[] _displayedTypes;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 58;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_implementations == null) RefreshImplementations();
            
            _serializedTypeProperty = property.FindPropertyRelative("_serializedType");
            if (_implementations.Contains(GetSerializedType()) == false)
                _serializedTypeProperty.stringValue = ClassTypeReference.NoneElement;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(position, label);
                DrawSplitter(position);
                
                int i = DrawPopup(position);
                if (i == 0)
                    _serializedTypeProperty.stringValue = ClassTypeReference.NoneElement;
                else
                    _serializedTypeProperty.stringValue = _implementations[i - 1].AssemblyQualifiedName;

                if (DrawButton(position)) RefreshImplementations();
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

        private int DrawPopup(Rect position)
        {
            var popupRect = new Rect(position.x + 6, position.y + 32, position.width - 38, 20);
            
            var selectedIndex = 0;
            
            var type = GetSerializedType();
            if (type != null) 
                selectedIndex = Array.IndexOf(_displayedTypes, type.FullName);

            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            
            return EditorGUI.Popup(popupRect, selectedIndex, _displayedTypes, style);
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
        
        private Type GetSerializedType()
        {
            if (string.IsNullOrEmpty(_serializedTypeProperty.stringValue)) return null;
            return CachedTypes.GetType(_serializedTypeProperty.stringValue);
        }

        private void RefreshImplementations()
        {
            if (attribute is SelectImplementationAttribute implAttribute)
            {
                _implementations = ReflectionUtility.GetSubtypes(implAttribute.FieldType,
                    t => !t.IsAbstract && !t.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
                _displayedTypes = _implementations.Select(t => t.FullName).Prepend(ClassTypeReference.NoneElement).ToArray();
            }
        }
    }
}