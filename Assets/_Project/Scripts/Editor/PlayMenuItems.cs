using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UntitledBallGame.GlobalStates;
using UntitledBallGame.SceneManagement;
using Object = UnityEngine.Object;

namespace UntitledBallGame.Editor
{
    static class PlayMenuItems
    {
        [MenuItem("Play/From main menu")]
        private static void MainMenu()
        {
            PlayAndLoadScenes(typeof(MainMenuState),
                AssetHelper.GetScriptableObject<SceneProfile>("MainMenuProfile"));
        }
        
        [MenuItem("Play/From game")]
        private static void Game()
        {
            PlayAndLoadScenes(typeof(GameplayState),
                AssetHelper.GetScriptableObject<SceneProfile>("GameProfile"));
        }

        private static void PlayAndLoadScenes(Type state, SceneProfile sceneProfile)
        {
            EditorApplication.EnterPlaymode();
            
            GameManager.editorInitialState = state;
            GameManager.editorSceneProfile = sceneProfile;

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            void OnPlayModeStateChanged(PlayModeStateChange change)
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                if (change != PlayModeStateChange.EnteredPlayMode) return;

                SceneManager.LoadSceneAsync(0);
            }
        }
    }
}