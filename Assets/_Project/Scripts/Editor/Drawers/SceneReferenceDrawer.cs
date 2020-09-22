using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        private SerializedProperty _sceneAssetProperty;
        private string[] _scenePaths;
        private string[] _displayedScenes;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_scenePaths == null)
            {
                _scenePaths = SceneHelper.GetScenePaths().ToArray();
                _displayedScenes = _scenePaths.Select(SceneHelper.GetNameFromPath).ToArray();
            }
            
            _sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                int i = DrawPopup(position, property);
                if (i == -1) return;
                _sceneAssetProperty.objectReferenceValue = EditorSceneHelper.GetAssetFromPath(_scenePaths[i]);
            }
        }
        
        private int DrawPopup(Rect position, SerializedProperty property)
        {
            var popupRect = new Rect(position.x + 6, position.y, position.width - 6, 20);
            
            var selectedIndex = 0;
            
            if (_sceneAssetProperty.objectReferenceValue != null)
            {
                if (property.GetTargetObject() is SceneReference sceneReference)
                    selectedIndex = Array.IndexOf(_scenePaths, sceneReference.ScenePath);
            }
            
            var style = new GUIStyle(EditorStyles.popup) {fixedHeight = 20};
            
            return EditorGUI.Popup(popupRect, selectedIndex, _displayedScenes, style);
        }
    }
}