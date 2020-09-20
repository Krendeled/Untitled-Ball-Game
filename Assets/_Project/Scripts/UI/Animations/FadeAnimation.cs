using System;
using DG.Tweening;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public class FadeAnimation : IAnimation
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

        [Range(0, 1), SerializeField] private float _alphaStart;
        [Range(0, 1), SerializeField] private float _alphaEnd;
        [SerializeField] private float _duration;

        public void Play(Action onComplete = null)
        {
            CanvasGroup.DOFade(_alphaEnd, _duration).From(_alphaStart).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}