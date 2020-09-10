using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UntitledBallGame.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneProfile", menuName = "Scene Profile")]
public class SceneProfile : ScriptableObject
{
	public List<SerializableScene> scenes = new List<SerializableScene>();
}
