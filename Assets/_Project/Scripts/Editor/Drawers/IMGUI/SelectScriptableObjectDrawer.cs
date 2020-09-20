using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.Serialization;

namespace UntitledBallGame.Editor.Drawers.IMGUI
{
    [CustomPropertyDrawer(typeof(SelectScriptableObjectAttribute))]
    public class SelectScriptableObjectDrawer : PropertyDrawer
    {
        private ScriptableObject[] _assets;
        private SerializedProperty _serializedProperty;
        private string[] _displayedAssets;

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
                }
                
                int i = DrawPopup(position);
                _serializedProperty.objectReferenceValue = _assets[i];
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
            
            var selectedIndex = 0;
            
            var obj = _serializedProperty.objectReferenceValue;
            if (obj != null) 
                selectedIndex = Array.IndexOf(_displayedAssets, obj.name);
            
            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};

            return EditorGUI.Popup(popupRect, selectedIndex, _displayedAssets, style);
        }

        private void RefreshAssets()
        {
            if (attribute is SelectScriptableObjectAttribute attr)
            {
                var paths = AssetDatabase.FindAssets($"t:{attr.ScriptableType}").Select(a => AssetDatabase.GUIDToAssetPath(a));
                _assets = paths.Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>).ToArray();
            }
        }

        private void RefreshDisplayedAssets()
        {
            _displayedAssets = _assets.Select(a => a.name).ToArray();
        }
    }
}