using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Malee.List;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UntitledBallGame.SceneManagement;

namespace UntitledBallGame.SceneManagement
{
	[CreateAssetMenu(fileName = "NewSceneProfile", menuName = "Scene Profile")]
    public class SceneProfile : ScriptableObject
    {
    	[Reorderable]
    	public SceneReferenceArray scenes;
    
#if UNITY_EDITOR
    	public void LoadScenesEditor()
        {
	        if (!Application.isEditor && Application.isPlaying) return;
    		
    		var loadedScenes = new Scene[EditorSceneManager.loadedSceneCount];
    		for (int i = 0; i < loadedScenes.Length; i++)
    		{
    			loadedScenes[i] = SceneManager.GetSceneAt(i);
    		}

    		for (int i = 0; i < scenes.Count; i++)
    		{
    			var s = EditorSceneManager.OpenScene(scenes[i].ScenePath, OpenSceneMode.Additive);
    			if (i == 0) SceneManager.SetActiveScene(s);
    		}

    		foreach (var scene in loadedScenes)
    		{
    			if (scenes.FirstOrDefault(s => s.ScenePath == scene.path) == null)
    				EditorSceneManager.CloseScene(scene, true);
    		}
        }
#endif
		public IEnumerator LoadScenesRuntime()
		{
			if (Application.isEditor && !Application.isPlaying) yield return null;

			for (int i = 0; i < scenes.Count; i++)
			{
				var ao = i == 0 ? 
					SceneManager.LoadSceneAsync(scenes[i].ScenePath) : 
					SceneManager.LoadSceneAsync(scenes[i].ScenePath, LoadSceneMode.Additive);

				yield return ao;
			}

			yield return null;
		}

    	[Serializable]
    	public class SceneReferenceArray : ReorderableArray<SceneReference> {}
    }
}
