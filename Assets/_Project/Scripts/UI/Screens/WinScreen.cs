using System;
using UnityEngine;
using UnityEngine.UI;
using UntitledBallGame.UI.ScreenAnimations;

namespace UntitledBallGame.UI.Screens
{
    public class WinScreen : UiScreenBase
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Buttons")] [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button goToMenuButton;

        public event Action RestartLevelPressed;
        public event Action NextLevelPressed;
        public event Action GoToMenuPressed;

        protected override void Awake()
        {
            base.Awake();
            Animation = new FadeScreenAnimation(canvasGroup);
        }

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