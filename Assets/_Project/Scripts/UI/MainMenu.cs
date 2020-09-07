using System;
using UnityEngine;
using UnityEngine.UI;
using UntitledBallGame.UI.Screens;

namespace UntitledBallGame.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Buttons")] [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        [Header("Screens")] [SerializeField] private LevelScreen levelScreen;
        [SerializeField] private OptionsScreen optionsScreen;

        public LevelScreen LevelScreen => levelScreen;
        public OptionsScreen OptionsScreen => optionsScreen;

        public event Action PlayPressed;
        public event Action OptionsPressed;
        public event Action QuitPressed;

        private void OnEnable()
        {
            playButton.onClick.AddListener(() => PlayPressed?.Invoke());
            optionsButton.onClick.AddListener(() => OptionsPressed?.Invoke());
            quitButton.onClick.AddListener(() => QuitPressed?.Invoke());
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
            PlayPressed = null;
            OptionsPressed = null;
            QuitPressed = null;
        }
    }
}