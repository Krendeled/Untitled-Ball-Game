using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledBallGame.SceneManagement
{
    public class SceneLoader
    {
        public float Progress { get; private set; }
        public bool IsDone { get; private set; } = true;

        private readonly MonoBehaviour _owner;
        private readonly List<Func<object>> _actions;

        public SceneLoader(MonoBehaviour owner)
        {
            _owner = owner;
            _actions = new List<Func<object>>();
        }

        public YieldInstruction Execute()
        {
            return _owner.StartCoroutine(ExecuteCoroutine());
        }

        private IEnumerator ExecuteCoroutine()
        {
            IsDone = false;
            Progress = 0;
            foreach (var action in _actions)
            {
                yield return action();
                Progress += 1f / _actions.Count;
            }

            IsDone = true;
        }

        #region Loading

        public SceneLoader Load(int buildIndex, LoadSceneMode mode = LoadSceneMode.Single, bool setActive = false)
        {
            _actions.Add(() =>
            {
                var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
                if (scene.isLoaded == false)
                {
                    var op = SceneManager.LoadSceneAsync(buildIndex, mode);
                    if (setActive)
                    {
                        op.completed += _ =>
                            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
                    }

                    return op;
                }

                return null;
            });
            return this;
        }

        public SceneLoader Load(int[] buildIndices, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (buildIndices == null || buildIndices.Length == 0)
                return this;

            foreach (var buildIndex in buildIndices)
            {
                Load(buildIndex, mode);
            }

            return this;
        }

        public SceneLoader ThenLoad(int buildIndex, LoadSceneMode mode = LoadSceneMode.Single, bool setActive = false)
        {
            return Load(buildIndex, mode, setActive);
        }

        public SceneLoader ThenLoad(int[] buildIndices, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Load(buildIndices, mode);
        }

        //public SceneLoader Load(string name, LoadSceneMode mode = LoadSceneMode.Single, bool setActive = false)
        //{
        //    actions.Add(() =>
        //    {
        //        var scene = SceneManager.GetSceneByName(name);
        //        if (scene.isLoaded == false)
        //        {
        //            var op = SceneManager.LoadSceneAsync(name, mode);
        //            if (setActive)
        //            {
        //                op.completed += _ => SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        //            }
        //            return op;
        //        }
        //        return null;
        //    });
        //    return this;
        //}

        //public SceneLoader Load(string[] names, LoadSceneMode mode = LoadSceneMode.Single)
        //{
        //    if (names == null || names.Length == 0)
        //        return this;

        //    foreach (var name in names)
        //    {
        //        Load(name, mode);
        //    }
        //    return this;
        //}

        //public SceneLoader ThenLoad(string name, LoadSceneMode mode = LoadSceneMode.Single, bool setActive = false)
        //{
        //    return Load(name, mode, setActive);
        //}

        //public SceneLoader ThenLoad(string[] names, LoadSceneMode mode = LoadSceneMode.Single)
        //{
        //    return Load(names, mode);
        //}

        #endregion

        #region Unloading

        public SceneLoader Unload(int buildIndex, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            _actions.Add(() =>
            {
                var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
                if (scene.IsValid() && scene.isLoaded)
                {
                    var op = SceneManager.UnloadSceneAsync(buildIndex, options);
                    return op;
                }

                return null;
            });
            return this;
        }

        public SceneLoader Unload(int[] buildIndices, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            if (buildIndices == null || buildIndices.Length == 0)
                return this;

            foreach (var buildIndex in buildIndices)
            {
                Unload(buildIndex, options);
            }

            return this;
        }

        public SceneLoader ThenUnload(int buildIndex, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            return Unload(buildIndex, options);
        }

        public SceneLoader ThenUnload(int[] buildIndices, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            return Unload(buildIndices, options);
        }

        //public SceneLoader Unload(string name, UnloadSceneOptions options = UnloadSceneOptions.None)
        //{
        //    actions.Add(() =>
        //    {
        //        var scene = SceneManager.GetSceneByName(name);
        //        if (scene.IsValid() && scene.isLoaded)
        //        {
        //            var op = SceneManager.UnloadSceneAsync(name, options);
        //            return op;
        //        }
        //        return null;
        //    });
        //    return this;
        //}

        //public SceneLoader Unload(string[] names, UnloadSceneOptions options = UnloadSceneOptions.None)
        //{
        //    if (names == null || names.Length == 0)
        //        return this;

        //    foreach (var name in names)
        //    {
        //        Unload(name);
        //    }
        //    return this;
        //}

        //public SceneLoader ThenUnload(string name, UnloadSceneOptions options = UnloadSceneOptions.None)
        //{
        //    return Unload(name, options);
        //}

        //public SceneLoader ThenUnload(string[] names, UnloadSceneOptions options = UnloadSceneOptions.None)
        //{
        //    return Unload(names, options);
        //}

        #endregion

        public SceneLoader InsertCallback(Func<object> callback)
        {
            if (callback != null)
                _actions.Add(callback);
            return this;
        }
    }
}