using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Drawers.UIElements
{
    public class SceneReferenceDrawer : DefaultPropertyDrawer<SceneReferenceDrawer>
    {
        private VisualElement _root;
        private SerializedProperty _sceneAssetProperty;
        private List<string> _scenePaths;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = GetRoot();

            _sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");

            SetupControls(property);
            
            return _root;
        }

        private void SetupControls(SerializedProperty property)
        {
            var sceneHolder = _root.Q<VisualElement>(className:"scene-holder");
            
            _scenePaths = SceneHelper.GetScenePaths().ToList();
            
            var selectedScene = _scenePaths[0];
            if (_sceneAssetProperty.objectReferenceValue != null)
            {
                if (property.GetTargetObject() is SceneReference sceneReference)
                    selectedScene = sceneReference.ScenePath;
            }

            var scenePopup = new PopupField<string>("Scene", _scenePaths, selectedScene,
                SceneHelper.GetNameFromPath,
                SceneHelper.GetNameFromPath);

            var sceneAssetField = _root.Q<ObjectField>("HiddenSceneAssetField");

            sceneAssetField.RegisterValueChangedCallback(evt => OnTextFieldChanged(evt, scenePopup));
            scenePopup.RegisterValueChangedCallback(evt => OnSceneSelected(evt, sceneAssetField));

            sceneHolder.Add(scenePopup);
            sceneHolder.Add(sceneAssetField);
        }

        private void OnTextFieldChanged(ChangeEvent<Object> evt, PopupField<string> popupField)
        {
            var objValue = EditorSceneHelper.GetPathFromAsset(evt.newValue as SceneAsset);
            if (popupField.value != objValue)
            {
                popupField.value = objValue;
            }
        }
        
        private void OnSceneSelected(ChangeEvent<string> evt, ObjectField textField)
        {
            if (string.IsNullOrEmpty(evt.newValue) == false)
                textField.value = EditorSceneHelper.GetAssetFromPath(evt.newValue);
        }
    }
}