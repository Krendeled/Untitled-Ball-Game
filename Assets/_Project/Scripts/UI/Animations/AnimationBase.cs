using System;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public abstract class AnimationBase : IAnimation
    {
        public GameObject Target { get; set; }
        public float Duration => _duration;
        [SerializeField] protected float _duration;

        [SerializeField] protected AnimationCurve _curve = new AnimationCurve(
            new Keyframe(0, 0, 0, 0, 0,0), 
            new Keyframe(1, 1, 0, 0, 0, 0));
        
        public abstract void Play(Action onComplete = null);
    }
}