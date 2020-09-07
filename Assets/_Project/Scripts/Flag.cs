using System;
using UnityEngine;

namespace UntitledBallGame
{
    public class Flag : MonoBehaviour
    {
        public event Action Hit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                Hit?.Invoke();
            }
        }

        private void OnDisable()
        {
            Hit = null;
        }
    }
}