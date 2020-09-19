using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UntitledBallGame.Editor.Drawers;
using UntitledBallGame.Editor.Editors;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;
using Object = UnityEngine.Object;

namespace UntitledBallGame.Editor.Drawers.UIElements
{
    public class SelectScriptableObjectDrawer : DefaultPropertyDrawer<SelectScriptableObjectDrawer>
    {
        private VisualElement _root;
        private List<ScriptableObject> _assets;
    
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = GetRoot();

            RefreshAssets();
            
            SetupControls(property);
            
            return _root;
        }

        private void SetupControls(SerializedProperty property)
        {
            var refreshButton = _root.Q<Button>("RefreshButton");
            refreshButton.clicked += RefreshAssets;
            
            _root.Q<Label>(className:"header").text = property.displayName;
            
            var assetsHolder = _root.Q<VisualElement>("AssetsHolder");

            var selectedValue = property.objectReferenceValue != null ? 
                property.objectReferenceValue as ScriptableObject : 
                _assets[0];
            
            var popup = new PopupField<ScriptableObject>("Asset", _assets, selectedValue,
                so => so.name, so => so.name);

            var objectField = assetsHolder.Q<ObjectField>("HiddenObjectField");
            objectField.BindProperty(property);
            objectField.RegisterValueChangedCallback(evt => OnObjectFieldChanged(evt.newValue, popup));

            popup.RegisterValueChangedCallback(evt => OnAssetSelected(evt.newValue, objectField));

            assetsHolder.Add(popup);
            
            var impLabel = _root.Q<Label>("AssetsLabel");
            impLabel.text = $"Found {_assets.Count()} assets";
        }

        private void OnObjectFieldChanged(Object newValue, PopupField<ScriptableObject> popupField)
        {
            var asset = newValue as ScriptableObject;

            if (asset != null && asset != popupField.value)
                popupField.value = asset;
        }

        private void OnAssetSelected(ScriptableObject newValue, ObjectField objectField)
        {
            objectField.value = newValue;
        }

        private void RefreshAssets()
        {
            if (attribute is SelectScriptableObjectAttribute attr)
            {
                _assets = new List<ScriptableObject>();
                var paths = AssetDatabase.FindAssets($"t:{attr.ScriptableType}").Select(a => AssetDatabase.GUIDToAssetPath(a));

                foreach (var path in paths)
                {
                    _assets.Add(AssetDatabase.LoadAssetAtPath<ScriptableObject>(path));
                }
            }
        }
    }
}