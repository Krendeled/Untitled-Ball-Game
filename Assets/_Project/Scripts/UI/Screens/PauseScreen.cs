using System;
using UnityEngine;
using UnityEngine.UI;

namespace UntitledBallGame.UI.Screens
{
    public class PauseScreen : ScreenBase
    {
        [Header("Buttons")] [SerializeField] private Button closePauseButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button goToMenuButton;
        [SerializeField] private Button quitGameButton;

        public event Action ClosePressed;
        public event Action RestartLevelPressed;
        public event Action GoToMenuPressed;
        public event Action QuitPressed;
        
        void OnEnable()
        {
            closePauseButton.onClick.AddListener(() => ClosePressed?.Invoke());
            restartButton.onClick.AddListener(() => RestartLevelPressed?.Invoke());
            goToMenuButton.onClick.AddListener(() => GoToMenuPressed?.Invoke());
            quitGameButton.onClick.AddListener(() => QuitPressed?.Invoke());
        }

        void OnDisable()
        {
            closePauseButton.onClick.RemoveAllListeners();
            restartButton.onClick.RemoveAllListeners();
            goToMenuButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.RemoveAllListeners();
            ClosePressed = null;
            RestartLevelPressed = null;
            GoToMenuPressed = null;
            QuitPressed = null;
        }
    }
}