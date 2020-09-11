using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(SerializableScene))]
    public class SerializableSceneDrawer : DefaultPropertyDrawer<SerializableSceneDrawer>
    {
        private VisualElement _root;
        private SerializedProperty _scenePathProperty;
        private List<string> _sceneNames;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = GetRoot();

            _scenePathProperty = property.FindPropertyRelative("_scenePath");

            SetupControls(property);
            
            return _root;
        }

        private void SetupControls(SerializedProperty property)
        {
            var sceneHolder = _root.Q<VisualElement>(className:"scene-holder");
            
            _sceneNames = SceneHelper.GetScenePaths().ToList();
            var selectedScene = _scenePathProperty.stringValue;
            if (string.IsNullOrEmpty(selectedScene))
                selectedScene = _sceneNames[0];

            var scenePopup = new PopupField<string>("Scene", _sceneNames, selectedScene,
                SceneHelper.GetNameFromPath,
                SceneHelper.GetNameFromPath);

            var scenePathField = _root.Q<TextField>("HiddenScenePathField");

            scenePathField.RegisterValueChangedCallback(evt => OnTextFieldChanged(evt, scenePopup));
            scenePopup.RegisterValueChangedCallback(evt => OnSceneSelected(evt, scenePathField));

            sceneHolder.Add(scenePopup);
            sceneHolder.Add(scenePathField);
        }

        private void OnTextFieldChanged(ChangeEvent<string> evt, PopupField<string> popupField)
        {
            if (string.IsNullOrEmpty(evt.newValue) == false && popupField.value != evt.newValue)
            {
                popupField.value = evt.newValue;
            }
        }
        
        private void OnSceneSelected(ChangeEvent<string> evt, TextField textField)
        {
            if (string.IsNullOrEmpty(evt.newValue) == false)
                textField.value = evt.newValue;
        }
    }
}