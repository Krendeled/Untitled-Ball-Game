using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UntitledBallGame.Utility;

namespace UntitledBallGame.InputManagement
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        public Controls Controls { get; private set; }

        public event Action<Vector2> CameraMoved;
        public event Action<float> CameraZoomed;

        private SmoothVector2 _cameraMovement;
        private SmoothFloat _cameraZoom;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _cameraMovement = new SmoothVector2();
            _cameraZoom = new SmoothFloat();

            Controls = new Controls();
        }

        private void OnEnable()
        {
            Controls.EditMode.CameraMovement.performed += OnCameraMovement;
            Controls.UI.Zoom.performed += OnCameraZoom;
        }

        private void OnDisable()
        {
            Controls.EditMode.CameraMovement.performed -= OnCameraMovement;
            Controls.UI.Zoom.performed -= OnCameraZoom;
        }

        public Vector2 GetCameraMovement()
        {
            return _cameraMovement.Evaluate(Controls.EditMode.CameraMovement.ReadValue<Vector2>());
        }

        private void OnCameraMovement(InputAction.CallbackContext context)
        {
            CameraMoved?.Invoke(_cameraMovement.Evaluate(context.ReadValue<Vector2>()));
        }

        private void OnCameraZoom(InputAction.CallbackContext context)
        {
            CameraZoomed?.Invoke(_cameraZoom.Evaluate(context.ReadValue<Vector2>().y));
        }

        public Vector2 GetPointerPosition()
        {
            return Controls.UI.Point.ReadValue<Vector2>();
        }

        public Vector2 GetPointerDelta()
        {
            return Controls.PlayMode.ChargeVector.ReadValue<Vector2>();
        }

        public bool IsPointerHeld()
        {
            return Controls.PlayMode.Press.ReadValue<float>() > 0;
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        public void HideCursor()
        {
            Cursor.visible = false;
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }

        private struct Point
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return $"({X}, {Y})";
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point pos);
#endif
    }
}