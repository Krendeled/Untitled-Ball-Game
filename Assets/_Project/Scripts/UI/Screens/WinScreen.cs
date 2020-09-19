using System;
using UnityEngine;
using UnityEngine.UI;

namespace UntitledBallGame.UI.Screens
{
    public class WinScreen : ScreenBase
    {
        [Header("Buttons")] [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button goToMenuButton;

        public event Action RestartLevelPressed;
        public event Action NextLevelPressed;
        public event Action GoToMenuPressed;

        private void OnEnable()
        {
            restartLevelButton.onClick.AddListener(() => RestartLevelPressed?.Invoke());
            nextLevelButton.onClick.AddListener(() => NextLevelPressed?.Invoke());
            goToMenuButton.onClick.AddListener(() => GoToMenuPressed?.Invoke());
        }

        private void OnDisable()
        {
            restartLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.RemoveAllListeners();
            goToMenuButton.onClick.RemoveAllListeners();
            RestartLevelPressed = null;
            NextLevelPressed = null;
            GoToMenuPressed = null;
        }
    }
}