using UnityEngine;

namespace UntitledBallGame.StateMachine
{
    public abstract class BaseState<TState> : IState
        where TState : class, IState
    {
        protected readonly StateMachine<TState> StateMachine;

        protected BaseState(StateMachine<TState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter()
        {
            Debug.Log($"Enter {GetType().Name}");
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void LateUpdate()
        {
        }

        public virtual void Exit()
        {
            Debug.Log($"Exit {GetType().Name}");
        }
    }
}