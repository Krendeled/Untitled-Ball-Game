using System;
using System.Collections.Generic;
using System.Linq;
using Malee.List;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UntitledBallGame.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneProfile", menuName = "Scene Profile")]
public class SceneProfile : ScriptableObject
{
	[Reorderable]
	public SceneReferenceArray scenes;

#if UNITY_EDITOR
	public void LoadScenes()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			var loadedScenes = new Scene[EditorSceneManager.loadedSceneCount];
			for (int i = 0; i < loadedScenes.Length; i++)
			{
				loadedScenes[i] = SceneManager.GetSceneAt(i);
			}

			for (int i = 0; i < scenes.Count; i++)
			{
				var s = EditorSceneManager.OpenScene(scenes[i].ScenePath, OpenSceneMode.Additive);
				if (i == 0)
					SceneManager.SetActiveScene(s);
			}

			foreach (var scene in loadedScenes)
			{
				if (scenes.FirstOrDefault(s => s.ScenePath == scene.path) == null)
					EditorSceneManager.CloseScene(scene, true);
			}
		}
		else
		{
			var loadedScenes = new Scene[SceneManager.sceneCount];
			for (int i = 0; i < loadedScenes.Length; i++)
			{
				loadedScenes[i] = SceneManager.GetSceneAt(i);
			}
		
			for (int i = 0; i < scenes.Count; i++)
			{
				SceneManager.LoadSceneAsync(scenes[i].ScenePath, LoadSceneMode.Additive);
				if (i == 0)
					SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenes[i].ScenePath));
			}

			foreach (var scene in loadedScenes)
			{
				if (scenes.FirstOrDefault(s => s.ScenePath == scene.path) == null)
					SceneManager.UnloadSceneAsync(scene);
			}
		}
	}
#endif
	
	[Serializable]
	public class SceneReferenceArray : ReorderableArray<SceneReference> {}
}
