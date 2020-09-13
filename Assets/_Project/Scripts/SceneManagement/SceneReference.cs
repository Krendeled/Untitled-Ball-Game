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
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] private Object _sceneAsset;
        private bool IsValidSceneAsset
        {
            get
            {
                if (_sceneAsset == null) return false;
                return _sceneAsset is SceneAsset;
            }
        }
#endif
        
        [SerializeField] private string _scenePath = string.Empty;
        public string ScenePath
        {
            get
            {
#if UNITY_EDITOR
                return EditorSceneHelper.GetPathFromAsset(_sceneAsset as SceneAsset);
#else
                return scenePath;
#endif
            }
            set
            {
                _scenePath = value;
#if UNITY_EDITOR
                _sceneAsset = EditorSceneHelper.GetAssetFromPath(_scenePath);
#endif
            }
        }

        public SceneReference()
        {
        }

        public SceneReference(string scenePath)
        {
            _scenePath = scenePath;
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            HandleBeforeSerialize();
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
            // Asset is invalid but have Path to try and recover from
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(_scenePath) == false)
            {
                _sceneAsset = EditorSceneHelper.GetAssetFromPath(_scenePath);
                if (_sceneAsset == null) _scenePath = string.Empty;
            
                // EditorSceneManager.MarkAllScenesDirty();
            }
            // Asset takes precedence and overwrites Path
            else
            {
                _scenePath = EditorSceneHelper.GetPathFromAsset(_sceneAsset as SceneAsset);
            }
        }

        private void HandleAfterDeserialize()
        {
            // Asset is valid, don't do anything - Path will always be set based on it when it matters
            if (IsValidSceneAsset) return;

            // Asset is invalid but have path to try and recover from
            if (string.IsNullOrEmpty(_scenePath)) return;
            
            _sceneAsset = EditorSceneHelper.GetAssetFromPath(_scenePath);
            // No asset found, path was invalid. Make sure we don't carry over the old invalid path
            if (_sceneAsset == null) _scenePath = string.Empty;

            // if (Application.isPlaying == false) EditorSceneManager.MarkAllScenesDirty();
        }
#endif

        public override string ToString()
        {
            return _sceneAsset == null ? "Missing scene" : ScenePath;
        }
    }
}