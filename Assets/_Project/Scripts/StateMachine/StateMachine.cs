using System;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledBallGame.StateMachine
{
    public class StateMachine<TState>
        where TState : class, IState
    {
        public TState CurrentState { get; private set; }

        private readonly Dictionary<Type, TState> _allStates;
        private readonly Stack<TState> _stateHistory;

        public StateMachine()
        {
            _stateHistory = new Stack<TState>();
            _allStates = new Dictionary<Type, TState>();
        }

        public void AddState(TState state)
        {
            _allStates.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : class, TState
        {
            SetState(typeof(T));
        }

        public void SetState(Type state)
        {
            _allStates.TryGetValue(state, out TState newState);

            if (newState == null)
            {
                Debug.Log($"[{nameof(StateMachine<TState>)}] There is no {typeof(TState).Name}.");
                return;
            }

            if (CurrentState != null)
            {
                CurrentState.Exit();
                _stateHistory.Push(CurrentState);
            }

            CurrentState = newState;

            CurrentState?.Enter();
        }

        public void PushState<T>() where T : class, TState
        {
            PushState(typeof(T));
        }

        public void PushState(Type state)
        {
            _allStates.TryGetValue(state, out TState newState);

            if (newState == null)
            {
                Debug.Log($"[{nameof(StateMachine<TState>)}] There is no {typeof(TState).Name}.");
                return;
            }

            _stateHistory.Push(CurrentState);
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void SetPreviousState()
        {
            if (_stateHistory.Count == 0)
                return;

            CurrentState?.Exit();

            CurrentState = _stateHistory.Pop();
        }

        public void Update() => CurrentState?.Update();
        public void FixedUpdate() => CurrentState?.FixedUpdate();
        public void LateUpdate() => CurrentState?.LateUpdate();
    }
}