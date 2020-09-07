using UnityEngine;

namespace UntitledBallGame
{
    [RequireComponent(typeof(LineRenderer), typeof(Rigidbody2D), typeof(BallController))]
    public class Trajectory : MonoBehaviour
    {
        [SerializeField] private int pathResolution = 20;

        private LineRenderer _lr;
        private BallController _controller;
        private Rigidbody2D _ballRb;
        private float _gravity;
        private float _radianAngle;
        private Vector2 _oldChargeVector;
        private Vector2 _chargeForce;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _controller = GetComponent<BallController>();
            _ballRb = GetComponent<Rigidbody2D>();
            _gravity = Mathf.Abs(Physics2D.gravity.y);
        }

        private void Update()
        {
            if (_controller.ChargeVector == Vector2.zero)
            {
                ClearTrajectory();
            }
            else if (_controller.ChargeVector != _oldChargeVector)
            {
                _chargeForce = _controller.CalculateForce();
                UpdateTrajectory();
                _oldChargeVector = _controller.ChargeVector;
            }
        }

        private void ClearTrajectory()
        {
            _lr.positionCount = 0;
        }

        private void UpdateTrajectory()
        {
            _lr.positionCount = pathResolution + 1;
            _lr.SetPositions(CalculateArcArray());
        }

        private Vector3[] CalculateArcArray()
        {
            var arcArray = new Vector3[pathResolution + 1];

            float angle = Vector2.Angle(Vector2.right, _chargeForce);
            _radianAngle = Mathf.Deg2Rad * angle;

            //Debug.Log("angle:" + angle);

            float maxDistance = Mathf.Pow(_chargeForce.magnitude * 0.57f / _ballRb.mass, 2) *
                                Mathf.Sin(2 * _radianAngle) /
                                _gravity;

            for (int i = 0; i <= pathResolution; i++)
            {
                float t = i / (float) pathResolution;
                arcArray[i] = CalculateArcPoint(t, maxDistance);
            }

            return arcArray;
        }

        private Vector3 CalculateArcPoint(float t, float maxDistance)
        {
            float x = t * maxDistance;
            float y = x * Mathf.Tan(_radianAngle) - _gravity * x * x /
                (2 * Mathf.Pow(_chargeForce.magnitude * 0.57f / _ballRb.mass, 2) *
                 Mathf.Pow(Mathf.Cos(_radianAngle), 2));
            return new Vector3(x, y) + transform.position;
        }
    }
}