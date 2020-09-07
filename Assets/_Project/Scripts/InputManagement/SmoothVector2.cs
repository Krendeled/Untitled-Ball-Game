using UnityEngine;

namespace UntitledBallGame.InputManagement
{
    public class SmoothVector2
    {
        private Vector2 _oldMovement;

        public Vector2 Evaluate(Vector2 value)
        {
            float dampValue = 0.2f;
            var newMovement = value;
            newMovement = Vector2.Lerp(_oldMovement, newMovement, dampValue);
            _oldMovement = newMovement;
            return newMovement;
        }
    }
}