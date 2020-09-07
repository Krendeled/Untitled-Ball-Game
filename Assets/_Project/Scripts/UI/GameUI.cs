using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UntitledBallGame.UI.Screens;

namespace UntitledBallGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Camera uiCamera;

        [Header("Buttons")] [SerializeField] private Button pauseButton;
        [SerializeField] private Button changeModeButton;

        [Header("Screens")] [SerializeField] private PauseScreen pauseScreen;
        [SerializeField] private EditingScreen editingScreen;
        [SerializeField] private WaitingScreen waitingScreen;
        [SerializeField] private WinScreen winScreen;

        public PauseScreen PauseScreen => pauseScreen;
        public EditingScreen EditingScreen => editingScreen;
        public WaitingScreen WaitingScreen => waitingScreen;
        public WinScreen WinScreen => winScreen;

        public event Action PausePressed;
        public event Action ChangeModePressed;

        private void OnEnable()
        {
            AddUiCameraToStack();
            pauseButton.onClick.AddListener(() => PausePressed?.Invoke());
            changeModeButton.onClick.AddListener(() => ChangeModePressed?.Invoke());
        }

        private void OnDisable()
        {
            RemoveUiCameraFromStack();
            pauseButton.onClick.RemoveAllListeners();
            changeModeButton.onClick.RemoveAllListeners();
            PausePressed = null;
            ChangeModePressed = null;
        }

        private void AddUiCameraToStack()
        {
            if (Camera.main != null)
            {
                var cameraData = Camera.main.GetUniversalAdditionalCameraData();
                cameraData.cameraStack.Add(uiCamera);
            }
            else
                Debug.Log($"[{nameof(GameUI)}] MainCamera is not in the scene.");
        }

        private void RemoveUiCameraFromStack()
        {
            if (Camera.main != null)
            {
                var cameraData = Camera.main.GetUniversalAdditionalCameraData();
                cameraData.cameraStack.Remove(uiCamera);
            }
            else
                Debug.Log($"[{nameof(GameUI)}] MainCamera is not in the scene.");
        }
    }
}