using UnityEngine.UIElements;

namespace UntitledBallGame.Editor
{
    public class CustomListView : ListView
    {
        protected override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);
        }
        
        public new class UxmlFactory : UxmlFactory<CustomListView, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}