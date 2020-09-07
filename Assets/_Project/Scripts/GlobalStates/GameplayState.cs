using System;
using UntitledBallGame.GameStates;
using UntitledBallGame.StateMachine;
using UntitledBallGame.UI;

namespace UntitledBallGame.GlobalStates
{
    public class GameplayState : GlobalStateBase
    {
        public StateMachine<GlobalStateBase> GlobalStateMachine => Context.StateMachine;

        private readonly Func<GameStateContext> _setupContextCallback;
        private readonly StateMachine<GameStateBase> _gameStateMachine;

        private CameraController _cameraController;
        private GameUI _gameUi;

        public GameplayState(GlobalStateContext context, StateMachine<GameStateBase> gameStateMachine,
            Func<GameStateContext> setupContextCallback) :
            base(context)
        {
            _gameStateMachine = gameStateMachine;
            _setupContextCallback = setupContextCallback;
        }

        public override void Enter()
        {
            base.Enter();

            var gameContext = _setupContextCallback();
            _cameraController = gameContext.CameraController;
            _gameUi = gameContext.GameUi;

            _gameStateMachine.SetState<EnteringGameState>();

            Context.InputManager.CameraZoomed += OnZoom;
            _gameUi.PausePressed += OnPause;
            _gameUi.ChangeModePressed += OnModeChanged;
        }

        public override void Exit()
        {
            base.Exit();

            Context.InputManager.Controls.EditMode.Disable();
            Context.InputManager.Controls.PlayMode.Disable();

            Context.InputManager.CameraZoomed -= OnZoom;
            _gameUi.PausePressed -= OnPause;
            _gameUi.ChangeModePressed -= OnModeChanged;
        }

        public override void Update()
        {
            base.Update();
            _gameStateMachine.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _gameStateMachine.FixedUpdate();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            _gameStateMachine.LateUpdate();
        }

        private void OnZoom(float value) => _cameraController.Zoom(value);

        private void OnPause() => StateMachine.PushState<PauseState>();

        private void OnModeChanged()
        {
            if (_gameStateMachine.CurrentState is EditState)
                _gameStateMachine.SetState<PlayState>();
            else
                _gameStateMachine.SetState<EditState>();
        }
    }
}