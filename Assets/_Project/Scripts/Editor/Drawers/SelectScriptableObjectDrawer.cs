using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UntitledBallGame.Editor.Drawers;
using UntitledBallGame.Editor.Editors;
using UntitledBallGame.Serialization;
using UntitledBallGame.Utility;

namespace UntitledBallGame.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    [CustomPropertyDrawer(typeof(SelectScriptableObjectAttribute))]
    public class SelectScriptableObjectDrawer : DefaultPropertyDrawer<SelectScriptableObjectDrawer>
    {
        private VisualElement _root;
        private List<Type> _implementations;
    
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _root = GetRoot();

            RefreshImplementations();
            
            SetupControls(property);
            
            return _root;
        }

        private void SetupControls(SerializedProperty property)
        {
            
        }

        private void RefreshImplementations()
        {
            if (attribute is SelectScriptableObjectAttribute attr)
            {
                _implementations = ReflectionUtility.GetSubtypes(attr.ScriptableType,
                    t => !t.IsAbstract);
            }
        }
    }
}