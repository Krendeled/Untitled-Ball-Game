using UnityEngine;

namespace UntitledBallGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float speed = 20f;
        [SerializeField] private float zoomSensitivity = 0.001f;
        [SerializeField, Range(0, 0.1f)] private float smoothness = 0.02f;
        [SerializeField, Range(0.1f, 10f)] private float defaultZoom;

        public Vector2 CameraPosition => camera == null ? default : camera.transform.position;

        private readonly float _zoomMin = 0.1f;
        private readonly float _zoomMax = 10f;

        private void Awake()
        {
            camera.orthographicSize = defaultZoom;
        }

        public void SmoothTranslateTo(Vector2 pos)
        {
            Vector2 curVel = Vector2.zero;
            camera.transform.position =
                (Vector3) Vector2.SmoothDamp(camera.transform.position, pos, ref curVel, smoothness, speed) +
                new Vector3(0, 0, camera.transform.position.z);
        }

        public void TranslateBy(Vector2 offset)
        {
            if (offset == Vector2.zero) return;
            camera.transform.position += (Vector3) offset * speed;
        }

        public void TranslateTo(Vector2 pos)
        {
            camera.transform.position = new Vector3(pos.x, pos.y, camera.transform.position.z);
        }

        public void Zoom(float value)
        {
            float newZoom = camera.orthographicSize - value * zoomSensitivity;
            camera.orthographicSize = Mathf.Clamp(newZoom, _zoomMin, _zoomMax);
            Debug.Log(value * zoomSensitivity);
        }

        public Bounds CalculateCameraBounds()
        {
            if (camera == null)
                return default;
            var size = new Vector2(camera.orthographicSize * camera.pixelWidth / camera.pixelHeight * 2,
                camera.orthographicSize * 2);
            return new Bounds(camera.transform.position, size);
        }
    }
}