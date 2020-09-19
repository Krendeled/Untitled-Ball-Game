using System;
using UnityEngine;
using UntitledBallGame.Serialization;
using UntitledBallGame.UI.Animations;

namespace UntitledBallGame.UI.Screens
{
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] protected GameObject screen;
        
        [SelectImplementation(typeof(IAnimation)), SerializeReference]
        protected IAnimation ShowAnimation;
        [SelectImplementation(typeof(IAnimation)), SerializeReference]
        protected IAnimation HideAnimation;

        protected virtual void Awake()
        {
            screen.SetActive(false);
        }

        public virtual object Show(Action onComplete = null)
        {
            if (ShowAnimation.Duration == 0)
            {
                screen.SetActive(true);
            }
            else if (!screen.activeSelf)
            {
                ShowAnimation?.Play(onComplete);
                return new WaitForSecondsRealtime(ShowAnimation.Duration);
            }

            return null;
        }

        public virtual object Hide(Action onComplete = null)
        {
            if (HideAnimation.Duration == 0)
            {
                screen.SetActive(false);
            }
            else if (screen.activeSelf)
            {
                HideAnimation?.Play(onComplete);
                return new WaitForSecondsRealtime(HideAnimation.Duration);
            }

            return null;
        }
    }
}