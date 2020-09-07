using DG.Tweening;
using UnityEngine;

namespace UntitledBallGame.UI.ScreenAnimations
{
    public class FadeScreenAnimation : IScreenAnimation
    {
        private readonly CanvasGroup _canvasGroup;

        public FadeScreenAnimation(CanvasGroup canvasGroup)
        {
            _canvasGroup = canvasGroup;
        }

        public void PlayHide(float duration, System.Action onComplete = null)
        {
            _canvasGroup.DOFade(0, duration).OnComplete(() =>
            {
                _canvasGroup.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }

        public void PlayShow(float duration, System.Action onComplete = null)
        {
            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.DOFade(1, duration).From(0).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}