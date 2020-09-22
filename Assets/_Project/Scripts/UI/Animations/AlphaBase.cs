using System;
using DG.Tweening;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public abstract class AlphaBase : AnimationBase
    {
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    if (Target.TryGetComponent(out CanvasGroup canvasGroup))
                        _canvasGroup = canvasGroup;
                    else
                        Debug.LogError($"CanvasGroup wasn't found on {Target.name}.");
                }

                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;
    }
}