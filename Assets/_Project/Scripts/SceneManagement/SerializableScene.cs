using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace UntitledBallGame.SceneManagement
{
    [Serializable]
    public class SerializableScene : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] private Object _sceneAsset = null;
        private bool IsValidSceneAsset
        {
            get
            {
                if (_sceneAsset == null)
                    return false;
                return _sceneAsset is SceneAsset;
            }
        }
#endif
        
        [SerializeField] private string _scenePath = string.Empty;
        public string ScenePath => _scenePath;
        
        public SerializableScene() { }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            //HandleBeforeSerialize();
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += HandleAfterDeserialize;
#endif
        }

#if UNITY_EDITOR
        private void HandleBeforeSerialize()
        {
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(_scenePath) == false)
            {
                _sceneAsset = EditorSceneHelper.GetAssetFromPath(_scenePath);
                if (_sceneAsset == null)
                    _scenePath = string.Empty;
            
                // EditorSceneManager.MarkAllScenesDirty();
            }
            else
            {
                _scenePath = EditorSceneHelper.GetPathFromAsset(_sceneAsset as SceneAsset);
            }
        }

        private void HandleAfterDeserialize()
        {
            if (IsValidSceneAsset) return;

            if (string.IsNullOrEmpty(_scenePath)) return;
            
            _sceneAsset = EditorSceneHelper.GetAssetFromPath(_scenePath);
            
            if (_sceneAsset == null)
                _scenePath = string.Empty;

            // if (Application.isPlaying == false)
            //     EditorSceneManager.MarkAllScenesDirty();
        }
#endif

        public override string ToString()
        {
#if UNITY_EDITOR
            return _sceneAsset == null ? "Missing scene" : _sceneAsset.name;
#else
            return _scenePath == null ? "Missing scene": SceneHelper.GetNameFromPath(_scenePath);
#endif
        }
    }
}