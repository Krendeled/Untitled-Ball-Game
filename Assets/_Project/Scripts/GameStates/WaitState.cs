using UnityEngine;
using UntitledBallGame.GlobalStates;

namespace UntitledBallGame.GameStates
{
    public class WaitState : GameStateBase
    {
        private Flag _flag;

        public WaitState(GameplayState parent, GameStateContext context) : base(parent, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Context.GameUi.WaitingScreen.Show();

            _flag = Object.FindObjectOfType<Flag>(true);
            _flag.Hit += OnFlagHit;
        }

        public override void Exit()
        {
            base.Exit();

            if (Context.GameUi != null)
                Context.GameUi.WaitingScreen.Hide();

            if (_flag != null)
                _flag.Hit -= OnFlagHit;
        }

        public override void Update()
        {
            base.Update();

            // when ball falls out of level
            if (Context.LevelBounds.ContainsObject(Context.BallController.BallPosition) == false)
            {
                // reset ball
                if (Context.BallController.BallPosition != Context.BallSpawner.Position)
                {
                    Context.BallController.FreezeBall();
                    TeleportBallToSpawn();
                }

                StateMachine.SetState<PlayState>();
            }
            // when ball stops
            else if (Context.BallController.BallVelocity == Vector2.zero)
            {
                StateMachine.SetState<PlayState>();
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            Context.CameraController.SmoothTranslateTo(Context.BallController.BallPosition);

            Vector2 newPos = Context.LevelBounds.GetObjectInBounds(Context.CameraController.CalculateCameraBounds());
            Context.CameraController.TranslateTo(newPos);
        }

        private void OnFlagHit()
        {
            Parent.GlobalStateMachine.SetState<WinState>();
        }

        private void TeleportBallToSpawn()
        {
            Context.BallController.BallPosition = Context.BallSpawner.Position;
        }
    }
}