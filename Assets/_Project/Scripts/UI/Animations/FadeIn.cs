﻿using System;
using DG.Tweening;

namespace UntitledBallGame.UI.Animations
{
    public class FadeIn : FadeBase
    {
        public override void Play(Action onComplete = null)
        {
            CanvasGroup.DOFade(1, _duration).From(0).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}