using UnityEngine;

namespace UntitledBallGame
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            var ball = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform);
            ball.transform.SetParent(null);
        }
    }
}