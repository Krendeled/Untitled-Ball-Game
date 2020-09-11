using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.Editor.Editors
{
    namespace UntitledBallGame.Editor
    {
        [CustomEditor(typeof(SceneProfile))]
        public class SceneProfileEditor : DefaultEditor<SceneProfileEditor>
        {
            private VisualTreeAsset _itemLayout;
            private StyleSheet _itemStyle;
            
            private VisualElement _root;
            private ListView _listView;

            public override VisualElement CreateInspectorGUI()
            {
                _root = GetRoot();

                _itemLayout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/_Project/Scripts/Editor/Templates/SceneProfileItem.uxml");

                _listView = _root.Q<ListView>();
                _listView.makeItem = MakeItem;
                _listView.bindItem = BindItem;

                var hiddenSizeField = _root.Q<IntegerField>("HiddenListSizeField");
                hiddenSizeField.RegisterValueChangedCallback(evt => UpdateListViewHeight(evt.newValue));
                
                var profile = serializedObject.targetObject as SceneProfile;
                if (profile != null)
                    UpdateListViewHeight(profile.scenes.Count);
                
                var addButton = _root.Q<Button>("AddButton");
                addButton.clicked += AddButtonOnClicked;
                var removeButton = _root.Q<Button>("RemoveButton");
                removeButton.clicked += RemoveButtonOnClicked;
                var loadButton = _root.Q<Button>("LoadButton");
                loadButton.clicked += LoadScenes;

                return _root;
            }

            private void UpdateListViewHeight(int childCount)
            {
                _listView.style.minHeight = childCount * _listView.resolvedItemHeight + 2;
            }
            
            private VisualElement MakeItem()
            {
                var element = _itemLayout.CloneTree();
                return element;
            }

            private void BindItem(VisualElement element, int index)
            {
                var propertyField = element.Q<PropertyField>(className: "property-field");
                propertyField.BindProperty(_listView.itemsSource[index] as SerializedProperty);
                propertyField.RegisterCallback<MouseDownEvent>(PropertyFieldOnMouseDown);
                
                var dragHandle = element.Q(className: "drag-handle");
                dragHandle.RegisterCallback<MouseDownEvent>(DragHandleOnMouseDown);
            }

            private void PropertyFieldOnMouseDown(MouseDownEvent evt)
            {
                evt.StopImmediatePropagation();
            }

            private void DragHandleOnMouseDown(MouseDownEvent evt)
            {
                int selectedIdx = -1;

                for (int i = 0; i < _listView.childCount; i++)
                {
                    if (_listView[i].worldBound.Contains(evt.mousePosition))
                        selectedIdx = i;
                }
                
                if (evt.ctrlKey == false)
                    _listView.ClearSelection();
                
                _listView.AddToSelection(selectedIdx);
            }

            private void RemoveButtonOnClicked()
            {
                var profile = serializedObject.targetObject as SceneProfile;

                if (profile == null)
                    return;

                foreach (var idx in _listView.selectedIndices)
                {
                    profile.scenes[idx] = null;
                }
                profile.scenes.RemoveAll(s => s == null);
                _listView.ClearSelection();
            }

            private void AddButtonOnClicked()
            {
                var profile = serializedObject.targetObject as SceneProfile;
                if (profile == null)
                    return;
                profile.scenes.Add(new SerializableScene());
            }

            private void LoadScenes()
            {
                var loadedScenes = new Scene[EditorSceneManager.loadedSceneCount];
                for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                {
                    loadedScenes[i] = SceneManager.GetSceneAt(i);
                }
                
                var profile = serializedObject.targetObject as SceneProfile;
                for (var i = 0; i < profile.scenes.Count; i++)
                {
                    var s = EditorSceneManager.OpenScene(profile.scenes[i].ScenePath, OpenSceneMode.Additive);
                    
                    if (i == 0)
                        SceneManager.SetActiveScene(s);
                }

                foreach (var scene in loadedScenes)
                {
                    if (profile.scenes.FirstOrDefault(s => s.ScenePath == scene.path) == null)
                        EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
    }
}