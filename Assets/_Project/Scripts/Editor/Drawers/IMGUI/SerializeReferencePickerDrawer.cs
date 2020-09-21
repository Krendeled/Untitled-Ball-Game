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
        private Type _fieldType;
        private Type _selectedType;
        private Type[] _implementations;
        private string[] _displayedTypes;
        private bool _propertyVisible;
        
        private float _mainPropertyHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 58;
            if (_propertyVisible)
                return totalHeight + _mainPropertyHeight + 6;
            return totalHeight;
        }
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(GetTitleRect(position), label);
                DrawSplitter(GetSplitterRect(position));

                if (_fieldType == null)
                {
                    _selectedType = property.GetManagedReferenceFullType();
                    _fieldType = property.GetManagedReferenceFieldType();
                    if (_fieldType == null)
                    {
                        Debug.LogError("_baseType is null.");
                        return;
                    }
                }
                
                if (_implementations == null || DrawButton(GetRefreshButtonRect(position)))
                {
                    RefreshImplementations();
                    RefreshDisplayedTypes();
                }

                int i = DrawPopup(GetPopupRect(position));
                if (i == 0)
                {
                    property.managedReferenceValue = null;
                }
                else if (_implementations[i - 1] != _selectedType)
                {
                    property.managedReferenceValue = Activator.CreateInstance(_implementations[i - 1]);
                }
                
                if (_selectedType != null)
                {
                    DrawPropertyFields(GetPropertyRect(position), property);
                    _propertyVisible = true;
                }
                else
                {
                    _propertyVisible = false;
                }
            }
        }

        private int GetSelectedIndex()
        {
            if (_selectedType == null) return 0;
            
            return Array.IndexOf(_implementations, _selectedType) + 1;
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
    
        private int DrawPopup(Rect rect)
        {
            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            return EditorGUI.Popup(rect, GetSelectedIndex(), _displayedTypes, style);
        }

        private void DrawPropertyFields(Rect rect, SerializedProperty property)
        {
            int startDepth = property.depth;
            _mainPropertyHeight = 0;
            
            if (property.propertyType != SerializedPropertyType.ManagedReference || !property.NextVisible(true)) 
                return;

            while (property.depth > startDepth)
            {
                var path = property.propertyPath;
                EditorGUI.PropertyField(rect, property);

                float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                rect.y += height;
                _mainPropertyHeight += height;

                if (property.NextVisible(false) == false)
                    return;
            }
        }

        #endregion
    }
}