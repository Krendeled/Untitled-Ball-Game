using UntitledBallGame.InputManagement;
using UntitledBallGame.SceneManagement;
using UntitledBallGame.StateMachine;
using UntitledBallGame.UI;

namespace UntitledBallGame.GameStates
{
    public class GameStateContext
    {
        public StateMachine<GameStateBase> StateMachine { get; set; }
        public GameManager GameManager { get; set; }
        public InputManager InputManager { get; set; }
        public GameSceneManager SceneManager { get; set; }

        public BallSpawner BallSpawner { get; set; }
        public BallController BallController { get; set; }
        public CameraController CameraController { get; set; }
        public LevelBounds LevelBounds { get; set; }
        public GameUI GameUi { get; set; }
    }
}