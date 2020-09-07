using UnityEngine;

namespace UntitledBallGame
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float maxPointerDistance = 10;
        [SerializeField] private float maxForce = 10;

        [SerializeField] private float angularVelocityThreshold = 1;
        //[SerializeField, Range(0.01f, 10f)] private float sensitivity = 1f;

        private Vector2 _changeVector;

        public Vector2 ChargeVector
        {
            get => _changeVector;
            set => _changeVector = ClampChargeVector(value);
        }

        public bool IsBallLaunched { get; private set; }
        public Vector2 BallVelocity => _rb.velocity;

        public Vector2 BallPosition
        {
            get => transform.position;
            set => transform.position = value;
        }

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (IsBallLaunched && BallVelocity == Vector2.zero)
                IsBallLaunched = false;

            ClampAngularVelocity();
        }

        public void LaunchBall()
        {
            _rb.AddForce(CalculateForce(), ForceMode2D.Impulse);
            ChargeVector = Vector2.zero;
            IsBallLaunched = true;
        }

        public void FreezeBall()
        {
            _rb.Sleep();
            _rb.rotation = 0;
        }

        public Vector2 CalculateForce()
        {
            float t = Mathf.InverseLerp(0, maxPointerDistance, ChargeVector.magnitude);
            return Vector2.Lerp(Vector2.zero, ChargeVector.normalized * -maxForce, t);
        }

        private void ClampAngularVelocity()
        {
            if (Mathf.Abs(_rb.angularVelocity) <= angularVelocityThreshold)
                _rb.angularVelocity = 0;
        }

        private Vector2 ClampChargeVector(Vector2 vector)
        {
            vector = Vector2.ClampMagnitude(vector, maxPointerDistance);
            vector = ClampChargeVectorRotation(vector);

            return vector;
        }

        private Vector2 ClampChargeVectorRotation(Vector2 v)
        {
            float dotProduct = Vector2.Dot(Vector2.down, v);

            if (dotProduct < 0)
            {
                if (Vector2.Angle(Vector2.left, v) < 90)
                {
                    v = new Vector2(Mathf.Cos(1), Mathf.Sin(0)) * v.magnitude;
                }
                else
                {
                    v = new Vector2(Mathf.Cos(-1), Mathf.Sin(0)) * v.magnitude;
                }
            }

            return v;
        }
    }
}