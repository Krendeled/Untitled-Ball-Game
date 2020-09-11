using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UntitledBallGame.SceneManagement;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ClassTypeReference))]
    [CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
    public class SelectImplementationDrawer : DefaultPropertyDrawer<SelectImplementationDrawer>
    {
        private VisualElement _root;
        private List<Type> _implementations;
        private SerializedProperty _serializedTypeProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = GetRoot();

            RefreshImplementations();

            _serializedTypeProperty = property.FindPropertyRelative("_serializedType");
            if (_implementations.Contains(GetSerializedType()) == false)
                _serializedTypeProperty.stringValue = ClassTypeReference.NoneElement;

            SetupControls(property);

            return _root;
        }

        private void SetupControls(SerializedProperty property)
        {
            var refreshButton = _root.Q<Button>("RefreshButton");
            refreshButton.clicked += RefreshImplementations;
            
            _root.Q<Label>("Title").text = property.displayName;
            
            var impHolder = _root.Q<VisualElement>("ImplementationHolder");

            var displayedTypes = _implementations.Select(t => t.FullName).Prepend(ClassTypeReference.NoneElement).ToList();
            var selectedValue = ClassTypeReference.NoneElement;

            var type = GetSerializedType();
            if (type != null)
                selectedValue = type.FullName;

            var impPopup = new PopupField<string>("Implementation", displayedTypes, selectedValue);

            var hiddenTypeField = _root.Q<TextField>("HiddenTypeField");
            hiddenTypeField.RegisterValueChangedCallback(evt => OnTextFieldChanged(evt.newValue, impPopup));
            
            impPopup.RegisterValueChangedCallback(evt => OnTypeSelected(evt.newValue, hiddenTypeField));

            impHolder.Add(impPopup);

            var impLabel = _root.Q<Label>("ImplementationsLabel");
            impLabel.text = $"Found {_implementations.Count()} implementations";
        }

        private void OnTextFieldChanged(string newValue, PopupField<string> popupField)
        {
            // if (newValue != popupField.value)
            //     popupField.value = newValue;
        }

        private void OnTypeSelected(string newValue, TextField textField)
        {
            var foundType = _implementations.FirstOrDefault(i => i.FullName == newValue);

            if (foundType != null)
                textField.value = foundType.AssemblyQualifiedName;
            else
                textField.value = newValue;
        }

        private Type GetSerializedType()
        {
            if (string.IsNullOrEmpty(_serializedTypeProperty.stringValue))
                return null;
            return CachedTypes.GetType(_serializedTypeProperty.stringValue);
        }

        private void RefreshImplementations()
        {
            if (attribute is SelectImplementationAttribute implAttribute)
            {
                _implementations = ReflectionUtility.GetSubtypes(implAttribute.FieldType,
                    t => !t.IsAbstract && !t.IsSubclassOf(typeof(UnityEngine.Object)));
            }
        }
    }
}