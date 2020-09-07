using System.Collections.Generic;
using UnityEngine;
using UntitledBallGame.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneProfile", menuName = "Scene Profile")]
public class SceneProfile : ScriptableObject
{
	[SerializeField] private List<SerializableScene> _scenes = new List<SerializableScene>();
}
