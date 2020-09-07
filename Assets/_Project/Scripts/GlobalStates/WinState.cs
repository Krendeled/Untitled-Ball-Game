using UnityEngine;
using UntitledBallGame.UI;

namespace UntitledBallGame.GlobalStates
{
    public class WinState : GlobalStateBase
    {
        private GameUI _uiManager;

        public WinState(GlobalStateContext context) :
            base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _uiManager = Object.FindObjectOfType<GameUI>(true);
            _uiManager.WinScreen.Show();

            _uiManager.WinScreen.GoToMenuPressed += OnGoToMenu;
            _uiManager.WinScreen.NextLevelPressed += OnNextLevel;
            _uiManager.WinScreen.RestartLevelPressed += OnRestartLevel;
        }

        public override void Exit()
        {
            base.Exit();

            if (_uiManager == null)
                return;

            _uiManager.WinScreen.Hide();

            _uiManager.WinScreen.GoToMenuPressed -= OnGoToMenu;
            _uiManager.WinScreen.NextLevelPressed -= OnNextLevel;
            _uiManager.WinScreen.RestartLevelPressed -= OnRestartLevel;
        }

        private void OnRestartLevel()
        {
            Context.SceneManager.ReloadCurrentLevel(afterLoading: () => StateMachine.SetState<GameplayState>());
        }

        private void OnNextLevel()
        {
            Context.SceneManager.LoadNextLevel(afterLoading: () => StateMachine.SetState<GameplayState>());
        }

        private void OnGoToMenu()
        {
            Context.SceneManager.LoadMainMenu(afterLoading: () => StateMachine.SetState<MainMenuState>());
        }
    }
}