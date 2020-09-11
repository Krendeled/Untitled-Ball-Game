using System.Linq;
using UnityEngine;
using UntitledBallGame.SceneManagement;
using UntitledBallGame.UI;

namespace UntitledBallGame.GlobalStates
{
    public class MainMenuState : GlobalStateBase
    {
        private MainMenu _mainMenu;

        public MainMenuState(GlobalStateContext context) :
            base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _mainMenu = Object.FindObjectOfType<MainMenu>(true);
            _mainMenu.LevelScreen.CreateLevelButtons(GameSceneManager.Levels.Select(scene => scene.Name));

            _mainMenu.PlayPressed += OnPlay;
            _mainMenu.QuitPressed += OnQuit;
            _mainMenu.OptionsPressed += OnOptions;
            _mainMenu.LevelScreen.LevelSelected += OnLevelSelect;

            // TODO: sub to options events
        }

        public override void Exit()
        {
            base.Exit();

            if (_mainMenu == null) return;

            _mainMenu.PlayPressed -= OnPlay;
            _mainMenu.QuitPressed -= OnQuit;
            _mainMenu.OptionsPressed -= OnOptions;
            _mainMenu.LevelScreen.LevelSelected -= OnLevelSelect;

            // TODO: unsub from options events
        }

        private void OnPlay() => _mainMenu.LevelScreen.Show();

        private void OnOptions() => _mainMenu.OptionsScreen.Show();

        private void OnQuit() => Context.GameManager.Quit();

        private void OnLevelSelect(string levelName)
        {
            Context.SceneManager.LoadLevelFromMenu(
                levelName,
                afterLoading: () => StateMachine.SetState<GameplayState>());
        }
    }
}