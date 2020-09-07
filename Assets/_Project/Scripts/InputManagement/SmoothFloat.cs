using UnityEngine;

namespace UntitledBallGame.InputManagement
{
    public class SmoothFloat
    {
        private float _oldValue;

        public float Evaluate(float value)
        {
            float dampValue = 0.2f;
            var newMovement = value;
            newMovement = Mathf.Lerp(_oldValue, newMovement, dampValue);
            _oldValue = newMovement;
            return newMovement;
        }
    }
}