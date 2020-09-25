using UnityEngine;

namespace UntitledBallGame
{
    [CreateAssetMenu(fileName = "New Level Item", menuName = "Level Item")]
    public class LevelItem : ScriptableObject
    {
        public int id;
        public Sprite icon;
        public GameObject prefab;
    }
}