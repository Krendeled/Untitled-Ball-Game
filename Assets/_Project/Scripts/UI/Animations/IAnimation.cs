using System;
using UnityEngine;

namespace UntitledBallGame.UI.Animations
{
    public interface IAnimation
    {
        GameObject Target { get; set; }
        
        float Duration { get; }
        
        void Play(Action onComplete = null);
    }
}