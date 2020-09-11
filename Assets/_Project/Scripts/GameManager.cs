using DG.Tweening;
using UnityEngine;
using UntitledBallGame.GameStates;
using UntitledBallGame.GlobalStates;
using UntitledBallGame.InputManagement;
using UntitledBallGame.SceneManagement;
using UntitledBallGame.Serialization;
using UntitledBallGame.StateMachine;
using UntitledBallGame.UI;
using UntitledBallGame.UI.Screens;

namespace UntitledBallGame
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SelectImplementation(typeof(GlobalStateBase)), SerializeField]
        private ClassTypeReference _initialState;

        private StateMachine<GlobalStateBase> _stateMachine;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            DOTween.Init();

            var inputManager = InputManager.Instance;

            TransitionScreen transitionScreen = null;

            var gameSceneManager = new GameSceneManager(this);

            gameSceneManager.SetDefaultCallbackBeforeLoading(() =>
            {
                transitionScreen = FindObjectOfType<TransitionScreen>(true);
                return transitionScreen.Show();
            });
            gameSceneManager.SetDefaultCallbackAfterLoading(() => transitionScreen.Hide());

            _stateMachine = new StateMachine<GlobalStateBase>();
            var gameStateMachine = new StateMachine<GameStateBase>();

            var globalContext = new GlobalStateContext
            {
                StateMachine = _stateMachine,
                GameManager = this,
                InputManager = inputManager,
                SceneManager = gameSceneManager
            };
            var gameContext = new GameStateContext
            {
                StateMachine = gameStateMachine,
                GameManager = this,
                InputManager = inputManager,
                SceneManager = gameSceneManager
            };

            GameStateContext SetupContextCallback()
            {
                gameContext.BallSpawner = FindObjectOfType<BallSpawner>(true);
                gameContext.BallController = FindObjectOfType<BallController>(true);
                gameContext.LevelBounds = FindObjectOfType<LevelBounds>(true);
                gameContext.CameraController = FindObjectOfType<CameraController>(true);
                gameContext.GameUi = FindObjectOfType<GameUI>(true);

                return gameContext;
            }

            var gameplayState = new GameplayState(globalContext, gameStateMachine, SetupContextCallback);

            gameStateMachine.AddState(new EnteringGameState(gameplayState, gameContext));
            gameStateMachine.AddState(new EditState(gameplayState, gameContext));
            gameStateMachine.AddState(new PlayState(gameplayState, gameContext));
            gameStateMachine.AddState(new WaitState(gameplayState, gameContext));
            
            _stateMachine.AddState(new MainMenuState(globalContext));
            _stateMachine.AddState(new PauseState(globalContext));
            _stateMachine.AddState(new WinState(globalContext));
            _stateMachine.AddState(gameplayState);
            
            _stateMachine.SetState(_initialState);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void LateUpdate()
        {
            _stateMachine.LateUpdate();
        }

        public void Pause(bool flag) => Time.timeScale = flag ? 0 : 1;

        public void Quit() => Application.Quit();
    }
}