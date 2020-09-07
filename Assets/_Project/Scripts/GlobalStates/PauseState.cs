using UnityEngine;
using UntitledBallGame.UI;

namespace UntitledBallGame.GlobalStates
{
    public class PauseState : GlobalStateBase
    {
        private GameUI _uiManager;

        public PauseState(GlobalStateContext context) :
            base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Context.GameManager.Pause(true);

            _uiManager = Object.FindObjectOfType<GameUI>(true);
            _uiManager.PauseScreen.Show();

            _uiManager.PauseScreen.ClosePressed += OnClose;
            _uiManager.PauseScreen.GoToMenuPressed += OnGoToMenu;
            _uiManager.PauseScreen.RestartLevelPressed += OnRestartLevel;
            _uiManager.PauseScreen.QuitPressed += OnQuit;
        }

        public override void Exit()
        {
            base.Exit();

            Context.GameManager.Pause(false);

            if (_uiManager == null)
                return;

            _uiManager.PauseScreen.Hide();

            _uiManager.PauseScreen.ClosePressed -= OnClose;
            _uiManager.PauseScreen.GoToMenuPressed -= OnGoToMenu;
            _uiManager.PauseScreen.RestartLevelPressed -= OnRestartLevel;
            _uiManager.PauseScreen.QuitPressed -= OnQuit;
        }

        private void OnRestartLevel()
        {
            Context.SceneManager.ReloadCurrentLevel(afterLoading: () => StateMachine.SetState<GameplayState>());
        }

        private void OnGoToMenu()
        {
            Context.SceneManager.LoadMainMenu(afterLoading: () => StateMachine.SetState<MainMenuState>());
        }

        private void OnClose()
        {
            StateMachine.SetPreviousState();
        }

        private void OnQuit()
        {
            Context.GameManager.Quit();
        }
    }
}