using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UntitledBallGame.Serialization;

namespace UntitledBallGame.SceneManagement
{
    public class SceneProfileManager : MonoBehaviour
    {
        [SelectScriptableObject(typeof(SceneProfile)), SerializeField]
        private SceneProfile _sceneProfile;
    }
}