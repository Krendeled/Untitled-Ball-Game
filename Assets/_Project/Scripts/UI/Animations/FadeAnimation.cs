using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public class FadeAnimation : IAnimation
    {
        private readonly CanvasGroup _canvasGroup;

        [Range(0, 1), SerializeField] private float _startValue;
        [Range(0, 1), SerializeField] private float _endValue;
        [MinValue(0), SerializeField] private float _duration;
        public float Duration => _duration;

        public FadeAnimation(GameObject target)
        {
            if (target.TryGetComponent(out CanvasGroup canvasGroup))
                _canvasGroup = canvasGroup;
            else
                Debug.LogError($"CanvasGroup wasn't found on {target.name}.");
        }
        
        public void Play(Action onComplete = null)
        {
            _canvasGroup.DOFade(_endValue, _duration).From(_startValue).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}