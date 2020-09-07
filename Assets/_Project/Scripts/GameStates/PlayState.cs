using UnityEngine;
using UnityEngine.InputSystem;
using UntitledBallGame.GlobalStates;

namespace UntitledBallGame.GameStates
{
    public class PlayState : GameStateBase
    {
        private bool _isBallPressed;

        public PlayState(GameplayState parent, GameStateContext context) : base(parent, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Context.CameraController.TranslateTo(Context.BallController.BallPosition);

            Context.InputManager.Controls.PlayMode.Enable();
            Context.InputManager.Controls.PlayMode.Press.started += OnPress;
            Context.InputManager.Controls.PlayMode.Press.canceled += OnRelease;
        }

        public override void Exit()
        {
            base.Exit();

            Context.InputManager.Controls.PlayMode.Disable();
            Context.InputManager.Controls.PlayMode.Press.started -= OnPress;
            Context.InputManager.Controls.PlayMode.Press.canceled -= OnRelease;
        }

        public override void Update()
        {
            base.Update();

            if (_isBallPressed && Context.InputManager.IsPointerHeld())
            {
                Vector2 delta = Context.InputManager.GetPointerDelta() * Time.deltaTime;

                if (delta != Vector2.zero)
                    Context.BallController.ChargeVector += delta;
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            Context.CameraController.SmoothTranslateTo(Context.BallController.BallPosition);

            Vector2 newPos = Context.LevelBounds.GetObjectInBounds(Context.CameraController.CalculateCameraBounds());
            Context.CameraController.TranslateTo(newPos);
        }

        private void OnPress(InputAction.CallbackContext context)
        {
            if (IsPointerOverPlayer(Context.CameraController.GetComponent<Camera>()) &&
                Context.BallController.IsBallLaunched == false)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                Context.InputManager.HideCursor();
#endif
                Context.BallController.ChargeVector = Vector2.zero;
                _isBallPressed = true;
            }
        }

        private void OnRelease(InputAction.CallbackContext context)
        {
            if (Context.BallController.ChargeVector == Vector2.zero)
                return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            Context.InputManager.ShowCursor();
#endif
            Context.BallController.LaunchBall();
            _isBallPressed = false;

            StateMachine.SetState<WaitState>();
        }

        private bool IsPointerOverPlayer(Camera camera)
        {
            return Physics2D.Raycast(camera.ScreenToWorldPoint(Context.InputManager.GetPointerPosition()),
                Vector2.zero,
                0,
                LayerMask.GetMask("Player"));
        }
    }
}