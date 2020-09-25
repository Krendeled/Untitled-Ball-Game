using System;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledBallGame.UI.Screens
{
    public class EditingScreen : ScreenBase
    {
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private Transform itemContainer;
        
        public event Action<int> ItemSelected;
        
        private List<LevelItemButton> _buttons;

        public void CreateItemButtons(IEnumerable<LevelItemDTO> items)
        {
            _buttons = new List<LevelItemButton>();
            foreach (var item in items)
            {
                var button = Instantiate(itemButtonPrefab, itemContainer);
                var itemButton = button.GetComponent<LevelItemButton>();
                itemButton.Icon.sprite = item.Icon;
                itemButton.AddListener(() => ItemSelected?.Invoke(item.Id));
                _buttons.Add(itemButton);
            }
        }
        
        private void OnDisable()
        {
            foreach (var button in _buttons)
            {
                button.RemoveAllListeners();
            }

            ItemSelected = null;
        }
    }
}