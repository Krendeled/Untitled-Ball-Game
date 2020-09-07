using UnityEngine;
using UntitledBallGame.GlobalStates;

namespace UntitledBallGame.GameStates
{
    public class EditState : GameStateBase
    {
        public EditState(GameplayState parent, GameStateContext context) : base(parent, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Context.InputManager.Controls.EditMode.Enable();

            Context.GameUi.EditingScreen.Show();

            if (Context.BallController.BallPosition != Context.BallSpawner.Position)
            {
                Context.BallController.FreezeBall();
                TeleportBallToSpawn();
            }

            Context.CameraController.TranslateTo(Context.BallController.BallPosition);
        }

        public override void Exit()
        {
            base.Exit();

            Context.InputManager.Controls.EditMode.Disable();

            Context.GameUi.EditingScreen.Hide();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            Context.CameraController.TranslateBy(Context.InputManager.GetCameraMovement() * Time.deltaTime);

            Vector2 newPos = Context.LevelBounds.GetObjectInBounds(Context.CameraController.CalculateCameraBounds());
            Context.CameraController.TranslateTo(newPos);
        }

        private void TeleportBallToSpawn()
        {
            Context.BallController.BallPosition = Context.BallSpawner.Position;
        }
    }
}