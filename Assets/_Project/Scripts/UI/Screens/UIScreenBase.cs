using System;
using UnityEngine;
using UntitledBallGame.UI.ScreenAnimations;

namespace UntitledBallGame.UI.Screens
{
    public abstract class UiScreenBase : MonoBehaviour
    {
        [SerializeField] protected GameObject screen;

        [Range(0, 10)] [SerializeField] protected float showDuration;
        [Range(0, 10)] [SerializeField] protected float hideDuration;

        public float ShowDuration => showDuration;
        public float HideDuration => hideDuration;

        protected IScreenAnimation Animation;

        protected virtual void Awake()
        {
            screen.SetActive(false);
            Animation = new BasicScreenAnimation(screen);
        }

        public virtual object Show(Action onComplete = null)
        {
            if (showDuration == 0)
            {
                screen.SetActive(true);
            }
            else if (!screen.activeSelf)
            {
                Animation?.PlayShow(showDuration, onComplete);
                return new WaitForSecondsRealtime(showDuration);
            }

            return null;
        }

        public virtual object Hide(Action onComplete = null)
        {
            if (hideDuration == 0)
            {
                screen.SetActive(false);
            }
            else if (screen.activeSelf)
            {
                Animation?.PlayHide(hideDuration, onComplete);
                return new WaitForSecondsRealtime(hideDuration);
            }

            return null;
        }
    }
}