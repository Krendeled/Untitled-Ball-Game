using System;
using Malee.List;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Editors
{
    namespace UntitledBallGame.Editor
    {
        [CustomEditor(typeof(SceneProfile))]
        public class SceneProfileEditor : UnityEditor.Editor
        {
            private ReorderableList _scenesList;
            private SerializedProperty _scenesProperty;
            private SceneProfile _targetObject;

            private void OnEnable()
            {
                _scenesProperty = serializedObject.FindProperty("scenes");
                _targetObject = serializedObject.targetObject as SceneProfile; 
                _scenesList = new ReorderableList(_scenesProperty);
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                
                _scenesList.DoLayoutList();
                if (DrawLoadButton())
                    _targetObject.LoadScenesEditor();
                
                serializedObject.ApplyModifiedProperties();
            }

            private bool DrawLoadButton()
            {
                return GUILayout.Button("Load");
            }
        }
        
        // [CustomEditor(typeof(SceneProfile))]
        // public class SceneProfileEditor : DefaultEditor<SceneProfileEditor>
        // {
        //     private VisualTreeAsset _itemLayout;
        //     private StyleSheet _itemStyle;
        //     
        //     private VisualElement _root;
        //     private ListView _listView;
        //
        //     private SceneProfile _targetObject;
        //     private SerializedProperty _scenesProperty;
        //
        //     public override VisualElement CreateInspectorGUI()
        //     {
        //         _root = GetRoot();
        //
        //         _scenesProperty = serializedObject.FindProperty("scenes");
        //         _targetObject = serializedObject.targetObject as SceneProfile;
        //         
        //         _itemLayout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
        //             "Assets/_Project/Scripts/Editor/Templates/SceneProfileItem.uxml");
        //
        //         _listView = _root.Q<ListView>();
        //         _listView.makeItem = MakeItem;
        //         _listView.bindItem = BindItem;
        //
        //         var hiddenSizeField = _root.Q<IntegerField>("HiddenListSizeField");
        //         hiddenSizeField.RegisterValueChangedCallback(evt => UpdateListViewHeight(evt.newValue));
        //         UpdateListViewHeight(_targetObject.scenes.Count);
        //         
        //         var addButton = _root.Q<Button>(className: "add-button");
        //         addButton.clicked += OnAddButtonClicked;
        //         var loadButton = _root.Q<Button>(className: "load-button");
        //         loadButton.clicked += OnLoadButtonClicked;
        //
        //         return _root;
        //     }
        //
        //     private void UpdateListViewHeight(int childCount)
        //     {
        //         _listView.style.minHeight = childCount * _listView.resolvedItemHeight + 2;
        //     }
        //     
        //     private VisualElement MakeItem()
        //     {
        //         var element = _itemLayout.CloneTree();
        //         return element;
        //     }
        //
        //     private void BindItem(VisualElement element, int index)
        //     {
        //         element.AddManipulator(new ListViewItemDragger<SceneReference>(_listView, _targetObject.scenes));
        //         
        //         var propertyField = element.Q<PropertyField>(className: "property-field");
        //         propertyField.BindProperty(_listView.itemsSource[index] as SerializedProperty);
        //
        //         var removeButton = element.Q<Button>(className: "remove-button");
        //         removeButton.clicked += () => OnRemoveButtonClicked(index);
        //     }
        //
        //     private void OnRemoveButtonClicked(int index)
        //     {
        //         _scenesProperty.DeleteArrayElementAtIndex(index);
        //         serializedObject.ApplyModifiedProperties();
        //     }
        //
        //     private void OnAddButtonClicked()
        //     {
        //         _scenesProperty.arraySize++;
        //         _scenesProperty.GetArrayElementAtIndex(_scenesProperty.arraySize - 1).FindPropertyRelative("_sceneAsset")
        //             .objectReferenceValue = EditorSceneHelper.GetAssetFromPath(EditorSceneHelper.GetPathFromBuildIndex(0));
        //         serializedObject.ApplyModifiedProperties();
        //     }
        //
        //     private void OnLoadButtonClicked()
        //     {
        //         _targetObject.LoadScenes();
        //     }
        // }
    }
}