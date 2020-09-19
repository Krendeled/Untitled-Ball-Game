using System;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledBallGame.UI.Screens
{
    public class LevelScreen : ScreenBase
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform parent;

        public event Action<string> LevelSelected;

        private List<LevelButton> _buttons;

        public void CreateLevelButtons(IEnumerable<string> levels)
        {
            _buttons = new List<LevelButton>();
            foreach (var level in levels)
            {
                var button = Instantiate(buttonPrefab, parent);
                var levelButton = button.GetComponent<LevelButton>();
                levelButton.Text = level;
                levelButton.AddListener(() => LevelSelected?.Invoke(level));
                _buttons.Add(levelButton);
            }
        }

        private void OnDisable()
        {
            foreach (var button in _buttons)
            {
                button.RemoveAllListeners();
            }

            LevelSelected = null;
        }
    }
}