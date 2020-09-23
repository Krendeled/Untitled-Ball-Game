using System;
using DG.Tweening;

namespace UntitledBallGame.UI.Animations
{
    public class FadeOut : AlphaBase
    {
        public override void Play(Action onComplete = null)
        {
            CanvasGroup.DOFade(0, _duration).From(1).SetEase(_curve).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}