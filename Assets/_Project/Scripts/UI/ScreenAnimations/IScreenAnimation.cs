namespace UntitledBallGame.UI.ScreenAnimations
{
    public interface IScreenAnimation
    {
        void PlayShow(float duration, System.Action onComplete = null);
        void PlayHide(float duration, System.Action onComplete = null);
    }
}