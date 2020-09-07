using UntitledBallGame.StateMachine;

namespace UntitledBallGame.GlobalStates
{
    public abstract class GlobalStateBase : BaseState<GlobalStateBase>
    {
        protected GlobalStateContext Context { get; }

        protected GlobalStateBase(GlobalStateContext context) :
            base(context.StateMachine)
        {
            Context = context;
        }
    }
}