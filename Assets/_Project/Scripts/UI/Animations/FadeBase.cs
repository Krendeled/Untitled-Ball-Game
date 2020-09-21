using System;
using DG.Tweening;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public abstract class FadeBase : IAnimation
    {
        public GameObject Target { get; set; }
        public float Duration => _duration;

        private CanvasGroup _canvasGroup;

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
        
        [SerializeField] protected float _duration;

        public abstract void Play(Action onComplete = null);
    }
}