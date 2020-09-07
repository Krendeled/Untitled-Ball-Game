using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledBallGame.SceneManagement
{
    public class GameSceneManager
    {
        private const string LevelsPath = "Assets/_Project/Scenes/Levels/";

        public static IReadOnlyList<LevelScene> Levels
        {
            get
            {
                if (_levels == null) _levels = GetLevels();
                return _levels;
            }
        }

        private static IReadOnlyList<LevelScene> _levels;

        public IReadOnlyList<Scene> LoadedScenes => _loadedScenes;
        private readonly List<Scene> _loadedScenes;

        public Scene? CurrentLevel { get; private set; }

        private Func<object> _defaultCallbackBeforeLoading;
        private Func<object> _defaultCallbackAfterLoading;

        private readonly MonoBehaviour _owner;

        public GameSceneManager(MonoBehaviour owner)
        {
            _owner = owner;

            _loadedScenes = GetLoadedScenes();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            var currentLvl = _loadedScenes.FirstOrDefault(s => s.path.Contains(LevelsPath));
            if (currentLvl.IsValid())
                CurrentLevel = currentLvl;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => _loadedScenes.Add(scene);

        private void OnSceneUnloaded(Scene scene) => _loadedScenes.Remove(scene);

        public void SetDefaultCallbackBeforeLoading(Func<object> callback) =>
            _defaultCallbackBeforeLoading = callback;

        public void SetDefaultCallbackAfterLoading(Func<object> callback) =>
            _defaultCallbackAfterLoading = callback;

        private void LoadLevel(string name, GameScene[] scenesToUnload = null, Action beforeLoading = null,
            Action afterLoading = null)
        {
            LevelScene? newLevel = Levels.FirstOrDefault(s => s.Path.Contains(name));

            int lvlBuildIndex = newLevel.Value.BuildIndex;

            var loader = new SceneLoader(_owner)
                .Load((int) GameScene.LevelTransition, LoadSceneMode.Additive);

            if (_defaultCallbackBeforeLoading != null)
                loader.InsertCallback(_defaultCallbackBeforeLoading);

            if (beforeLoading != null)
                loader.InsertCallback(() =>
                {
                    beforeLoading();
                    return null;
                });

            if (scenesToUnload != null && scenesToUnload.Length > 0)
                loader.ThenUnload(scenesToUnload.Select(s => (int) s).ToArray());

            loader.ThenLoad((int) GameScene.Base, LoadSceneMode.Additive);
            loader.ThenLoad(lvlBuildIndex, LoadSceneMode.Additive, true);
            loader.ThenLoad((int) GameScene.GameUi, LoadSceneMode.Additive);

            loader.InsertCallback(() =>
            {
                CurrentLevel = SceneManager.GetSceneByBuildIndex(lvlBuildIndex);
                return null;
            });

            if (afterLoading != null)
                loader.InsertCallback(() =>
                {
                    afterLoading();
                    return null;
                });

            if (_defaultCallbackAfterLoading != null)
                loader.InsertCallback(_defaultCallbackAfterLoading);

            loader.ThenUnload((int) GameScene.LevelTransition)
                .Execute();
        }

        public void LoadLevelFromMenu(string name, Action beforeLoading = null, Action afterLoading = null)
        {
            LoadLevel(name, new[] {GameScene.MainMenu}, beforeLoading, afterLoading);
        }

        public void LoadNextLevel(Action beforeLoading = null, Action afterLoading = null)
        {
            var nextLevel = GetNextLevel();
            if (nextLevel.HasValue && string.IsNullOrEmpty(nextLevel.Value.Path) == false)
                LoadLevel(nextLevel.Value.Path, new[]
                    {
                        (GameScene) CurrentLevel.Value.buildIndex,
                        GameScene.GameUi, GameScene.Base
                    },
                    beforeLoading, afterLoading);
        }

        public void ReloadCurrentLevel(Action beforeLoading = null, Action afterLoading = null)
        {
            if (CurrentLevel == null) return;

            LoadLevel(CurrentLevel.Value.path, new[]
                {
                    (GameScene) CurrentLevel.Value.buildIndex,
                    GameScene.GameUi, GameScene.Base
                },
                beforeLoading, afterLoading);
        }

        public void LoadMainMenu(Action beforeLoading = null, Action afterLoading = null)
        {
            var loader = new SceneLoader(_owner)
                .Load((int) GameScene.LevelTransition, LoadSceneMode.Additive);

            if (_defaultCallbackBeforeLoading != null)
                loader.InsertCallback(_defaultCallbackBeforeLoading);

            if (beforeLoading != null)
                loader.InsertCallback(() =>
                {
                    beforeLoading();
                    return null;
                });

            loader.ThenUnload((int) GameScene.GameUi);
            loader.ThenUnload((int) GameScene.Base);

            if (CurrentLevel.HasValue)
                loader.ThenUnload(CurrentLevel.Value.buildIndex);

            loader.ThenLoad((int) GameScene.MainMenu, LoadSceneMode.Additive);

            loader.InsertCallback(() =>
            {
                CurrentLevel = null;
                return null;
            });

            if (afterLoading != null)
                loader.InsertCallback(() =>
                {
                    afterLoading();
                    return null;
                });

            if (_defaultCallbackAfterLoading != null)
                loader.InsertCallback(_defaultCallbackAfterLoading);

            loader.ThenUnload((int) GameScene.LevelTransition)
                .Execute();
        }

        private LevelScene? GetNextLevel()
        {
            if (CurrentLevel == null) return null;

            for (int i = 0; i < Levels.Count - 1; i++)
            {
                if (Levels[i].BuildIndex == CurrentLevel.Value.buildIndex)
                    return Levels[i + 1];
            }

            return null;
        }

        private List<Scene> GetLoadedScenes()
        {
            var scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }

            return scenes;
        }

        private static List<LevelScene> GetLevels()
        {
            var scenes = new List<LevelScene>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                if (scenePath.StartsWith(LevelsPath))
                    scenes.Add(new LevelScene(i, scenePath));
            }

            return scenes;
        }
    }

    internal class LoadingCallbacks
    {
        public Func<object> BeforeLoading { get; set; }
        public Func<object> AfterLoading { get; set; }

        public LoadingCallbacks()
        {
            BeforeLoading = null;
            AfterLoading = null;
        }
    }

    public readonly struct LevelScene
    {
        public int BuildIndex { get; }
        public string Path { get; }
        public string Name { get; }

        public LevelScene(int buildIndex, string path = null)
        {
            BuildIndex = buildIndex;

            if (buildIndex < 0)
            {
                Path = null;
                Name = null;
            }
            else
            {
                if (path == null)
                    Path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
                else
                    Path = path;
                Name = SceneHelper.GetNameFromPath(path);
            }
        }
    }
}