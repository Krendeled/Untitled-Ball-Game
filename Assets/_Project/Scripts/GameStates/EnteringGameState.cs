using UntitledBallGame.GlobalStates;

namespace UntitledBallGame.GameStates
{
    public class EnteringGameState : GameStateBase
    {
        public EnteringGameState(GameplayState parent, GameStateContext context) : base(parent, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Context.CameraController.TranslateTo(Context.BallController.BallPosition);

            StateMachine.SetState<EditState>();
        }
    }
}