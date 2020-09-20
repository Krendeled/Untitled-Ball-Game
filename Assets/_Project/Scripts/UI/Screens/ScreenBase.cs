using System;
using UnityEngine;
using UntitledBallGame.Serialization;
using UntitledBallGame.UI.Animations;

namespace UntitledBallGame.UI.Screens
{
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] protected GameObject screen;
        
        [SerializeReference, SerializeReferencePicker]
        protected IAnimation ShowAnimation;
        [SerializeReference, SerializeReferencePicker]
        protected IAnimation HideAnimation;

        protected virtual void Awake()
        {
            screen.SetActive(false);
            if (ShowAnimation != null) ShowAnimation.Target = screen;
            if (HideAnimation != null) HideAnimation.Target = screen;
        }

        public object Show(Action onComplete = null)
        {
            if (ShowAnimation == null || ShowAnimation.Duration == 0)
            {
                screen.SetActive(true);
                onComplete?.Invoke();
                return null;
            }
            
            if (!screen.activeSelf)
            {
                screen.SetActive(true);
                ShowAnimation.Play(onComplete);
                return new WaitForSecondsRealtime(ShowAnimation.Duration);
            }

            return null;
        }

        public object Hide(Action onComplete = null)
        {
            onComplete += () => screen.SetActive(false);
            if (HideAnimation == null || HideAnimation.Duration == 0)
            {
                onComplete?.Invoke();
                return null;
            }

            if (screen.activeSelf)
            {
                HideAnimation?.Play(onComplete);
                return new WaitForSecondsRealtime(HideAnimation.Duration);
            }

            return null;
        }
    }
}