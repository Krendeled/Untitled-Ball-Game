using UnityEngine;
using UntitledBallGame.UI.ScreenAnimations;

namespace UntitledBallGame.UI.Screens
{
    public class TransitionScreen : UiScreenBase
    {
        [SerializeField] private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            Animation = new FadeScreenAnimation(canvasGroup);
        }
    }
}