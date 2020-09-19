using System;

namespace UntitledBallGame.UI.Animations
{
    public interface IAnimation
    {
        float Duration { get; }
        
        void Play(Action onComplete = null);
    }
}