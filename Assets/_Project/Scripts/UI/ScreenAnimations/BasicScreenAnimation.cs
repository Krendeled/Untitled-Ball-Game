using UnityEngine;

namespace UntitledBallGame.UI.ScreenAnimations
{
    public class BasicScreenAnimation : IScreenAnimation
    {
        private readonly GameObject _screen;

        public BasicScreenAnimation(GameObject screen)
        {
            _screen = screen;
        }

        public void PlayHide(float duration, System.Action onComplete = null)
        {
            _screen.SetActive(false);
        }

        public void PlayShow(float duration, System.Action onComplete = null)
        {
            _screen.SetActive(true);
        }
    }
}