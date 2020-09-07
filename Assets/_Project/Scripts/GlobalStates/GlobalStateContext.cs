using UntitledBallGame.InputManagement;
using UntitledBallGame.SceneManagement;
using UntitledBallGame.StateMachine;

namespace UntitledBallGame.GlobalStates
{
    public class GlobalStateContext
    {
        public StateMachine<GlobalStateBase> StateMachine { get; set; }
        public GameManager GameManager { get; set; }
        public InputManager InputManager { get; set; }
        public GameSceneManager SceneManager { get; set; }
    }
}