using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UntitledBallGame.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;

        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        public void AddListener(Action action)
        {
            if (action != null)
                button.onClick.AddListener(action.Invoke);
        }

        public void RemoveAllListeners()
        {
            button.onClick.RemoveAllListeners();
        }

        private void OnDisable()
        {
            RemoveAllListeners();
        }
    }
}