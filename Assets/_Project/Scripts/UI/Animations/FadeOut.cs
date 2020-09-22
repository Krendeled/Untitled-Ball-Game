using System;
using DG.Tweening;

namespace UntitledBallGame.UI.Animations
{
    public class FadeOut : AlphaBase
    {
        public override void Play(Action onComplete = null)
        {
            CanvasGroup.DOFade(1, _duration).From(0).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}