using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;
using ReflectionUtility = UntitledBallGame.Utility.ReflectionUtility;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SerializeReferencePickerAttribute))]
    public class SerializeReferencePickerDrawer : PropertyDrawer
    {
        private Type _fieldType;
        private Type _selectedType;
        private Type[] _implementations;
        private string[] _displayedTypes;

        private float _mainPropertyHeight;

        private readonly List<SerializedProperty> _subProperties = new List<SerializedProperty>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 58;
            return totalHeight + _mainPropertyHeight;
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                Debug.LogError($"{property.name} is not a managed reference.");
                return;
            }
            
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(GetTitleRect(position), label);
                DrawSplitter(GetSplitterRect(position));

                if (_fieldType == null)
                {
                    _fieldType = property.GetManagedReferenceFieldType();
                    if (_fieldType == null)
                    {
                        Debug.LogError("Field type is null.");
                        return;
                    }
                }

                if (_implementations == null || DrawButton(GetRefreshButtonRect(position)))
                {
                    RefreshImplementations();
                    RefreshDisplayedTypes();
                }
                
                _selectedType = property.GetManagedReferenceFullType();

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    int i = DrawPopup(GetPopupRect(position), GetSelectedIndex());

                    if (check.changed)
                    {
                        if (i == 0)
                        {
                            property.managedReferenceValue = null;
                            _selectedType = null;
                            _subProperties.Clear();
                        }
                        else
                        {
                            _selectedType = _implementations[i - 1];
                            property.managedReferenceValue = Activator.CreateInstance(_selectedType);
                        }
                    }
                }

                if (_selectedType != null)
                {
                    RefreshSubProperties(property);
                    _mainPropertyHeight = DrawPropertyFields(GetPropertyRect(position));
                }
            }
        }

        private void RefreshImplementations()
        {
            _implementations = ReflectionUtility.GetSubtypes(_fieldType, 
                t => 
                    !t.IsAbstract && 
                    !t.IsSubclassOf(typeof(UnityEngine.Object)) && 
                    t.GetConstructor(Type.EmptyTypes) != null).ToArray();
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

        private void RefreshSubProperties(SerializedProperty property)
        {
            var currentProperty = property.Copy();
            int startDepth = currentProperty.depth;

            if (currentProperty.propertyType != SerializedPropertyType.ManagedReference || !currentProperty.NextVisible(true)) 
                return;

            _subProperties.Clear();
            
            while (currentProperty.depth > startDepth)
            {
                _subProperties.Add(currentProperty.Copy());
                if (currentProperty.NextVisible(false) == false) return;
            }
        }

        private int GetSelectedIndex()
        {
            if (_selectedType == null) return 0;
            int i = Array.IndexOf(_implementations, _selectedType);
            if (i == -1) return 0;
            return i + 1;
        }

        #region Get rects

        private Rect GetTitleRect(Rect position)
        {
            return new Rect(position.x, position.y, position.width, 22);
        }
        
        private Rect GetSplitterRect(Rect position)
        {
            return new Rect(position.x + 2, position.y + 22, position.width - 4, 2);
        }

        private Rect GetRefreshButtonRect(Rect position)
        {
            return new Rect(
                position.x + position.width - 26,
                position.y + 24 + 6,
                20,
                20);
        }
        
        private Rect GetPopupRect(Rect position)
        {
            return new Rect(
                position.x + 6, 
                position.y + 24 + 6, 
                position.width - 38, 
                EditorGUIUtility.singleLineHeight);
        }

        private Rect GetPropertyRect(Rect position)
        {
            return new Rect(
                position.x + 6, 
                position.y + 58, 
                position.width - 12, 
                EditorGUIUtility.singleLineHeight);
        }

        #endregion

        #region Draw calls

        private void DrawBackground(Rect rect)
        {
            var color = new Color(0f, 0f, 0f, 0.2f);
            EditorGUI.DrawRect(rect, color);
        }
    
        private void DrawTitle(Rect rect, GUIContent label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUI.LabelField(rect, label, style);
        }
    
        private void DrawSplitter(Rect rect)
        {
            var color = new Color(255f, 255f, 255f, 0.1f);
            EditorGUI.DrawRect(rect, color);
        }
    
        private bool DrawButton(Rect rect)
        {
            var icon = EditorGUIUtility.IconContent("d_Refresh");
            var style = new GUIStyle(GUI.skin.button) 
            {
                padding = new RectOffset(0, 0, 0, 0), 
                margin = new RectOffset(0, 0, 0, 0)
            };
            return GUI.Button(rect, icon, style);
        }
    
        private int DrawPopup(Rect rect, int selectedIndex)
        {
            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            return EditorGUI.Popup(rect, selectedIndex, _displayedTypes, style);
        }

        private float DrawPropertyFields(Rect rect)
        {
            float height = _subProperties == null ? 0 : EditorGUIUtility.standardVerticalSpacing;

            foreach (var subProperty in _subProperties)
            {
                EditorGUI.PropertyField(rect, subProperty);
                float h = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                rect.y += h;
                height += h;
            }

            return height;
        }

        #endregion
    }
}