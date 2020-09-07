using UntitledBallGame.GlobalStates;
using UntitledBallGame.StateMachine;

namespace UntitledBallGame.GameStates
{
    public abstract class GameStateBase : BaseState<GameStateBase>
    {
        protected GameplayState Parent { get; }
        protected GameStateContext Context { get; }

        protected GameStateBase(GameplayState parent, GameStateContext context) :
            base(context.StateMachine)
        {
            Parent = parent;
            Context = context;
        }
    }
}