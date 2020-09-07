using UnityEngine;

namespace UntitledBallGame.UI
{
    public class BallKnob : MonoBehaviour
    {
        [SerializeField] private BallController ballController;

        void Update()
        {
            if (ballController.ChargeVector != Vector2.zero)
                transform.localPosition = transform.InverseTransformVector(ballController.ChargeVector);
            else
                transform.localPosition = ballController.BallPosition;
        }
    }
}