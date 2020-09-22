using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ScriptableObjectPickerAttribute))]
    public class ScriptableObjectPickerDrawer : PropertyDrawer
    {
        private ScriptableObject[] _assets;
        private SerializedProperty _serializedProperty;
        private string[] _displayedAssets;
        private ScriptableObject _selectedObject;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 58;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _serializedProperty = property;
            
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                DrawBackground(position);
                DrawTitle(position, label);
                DrawSplitter(position);

                if (_assets == null || DrawButton(position))
                {
                    RefreshAssets();
                    RefreshDisplayedAssets();
                    _selectedObject = _serializedProperty.objectReferenceValue as ScriptableObject;
                }
                
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    int i = DrawPopup(position, GetSelectedIndex());
                    
                    if (check.changed) _serializedProperty.objectReferenceValue = _assets[i];
                }
            }
        }
        
        private void RefreshAssets()
        {
            if (attribute is ScriptableObjectPickerAttribute attr)
            {
                var paths = AssetDatabase.FindAssets($"t:{attr.ScriptableType}").Select(a => AssetDatabase.GUIDToAssetPath(a));
                _assets = paths.Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>).ToArray();
            }
        }

        private void RefreshDisplayedAssets()
        {
            _displayedAssets = _assets.Select(a => a.name).ToArray();
        }

        private int GetSelectedIndex()
        {
            return _selectedObject == null ? 0 : Array.IndexOf(_assets, _selectedObject);
        }

        #region Draw calls

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

        private int DrawPopup(Rect position, int selectedIndex)
        {
            var popupRect = new Rect(position.x + 6, position.y + 32, position.width - 38, 20);

            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};

            return EditorGUI.Popup(popupRect, selectedIndex, _displayedAssets, style);
        }

        #endregion
    }
}