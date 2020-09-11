using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UntitledBallGame.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneProfile", menuName = "Scene Profile")]
public class SceneProfile : ScriptableObject
{
	public List<SerializableScene> scenes = new List<SerializableScene>();
	
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

			foreach (var scene in scenes)
			{
				var s = EditorSceneManager.OpenScene(scene.ScenePath, OpenSceneMode.Additive);
			}

			foreach (var scene in loadedScenes)
			{
				if (scene.path != SceneManager.GetActiveScene().path)
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
		
			foreach (var scene in scenes)
			{
				SceneManager.LoadSceneAsync(scene.ScenePath, LoadSceneMode.Additive);
			}

			foreach (var scene in loadedScenes)
			{
				if (scene.path != SceneManager.GetActiveScene().path)
					SceneManager.UnloadSceneAsync(scene);
			}
		}
	}
#endif
}
