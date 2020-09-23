using System;
using DG.Tweening;
using UnityEngine.Scripting.APIUpdating;

namespace UntitledBallGame.UI.Animations
{
    [MovedFrom(true, "UntitledBallGame.UI.Animations", "UntitledBallGame.UI", "FadeAnimation")]
    public class FadeIn : AlphaBase
    {
        public override void Play(Action onComplete = null)
        {
            CanvasGroup.DOFade(1, _duration).From(0).SetEase(_curve).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}