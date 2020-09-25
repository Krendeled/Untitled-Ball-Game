using System;
using UnityEngine;
using UnityEngine.UI;

namespace UntitledBallGame.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelItemButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        public Image Icon
        {
            get => button.image;
            set => button.image = value;
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