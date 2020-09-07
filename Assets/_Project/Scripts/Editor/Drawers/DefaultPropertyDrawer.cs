using UnityEditor;

namespace UntitledBallGame.Editor.Drawers
{
    public abstract class DefaultPropertyDrawer<T> : PropertyDrawer
    {
        protected readonly string _uxmlPath = AssetHelper.GetPathFromUxmlName(typeof(T).Name);
        protected readonly string _ussPath = AssetHelper.GetPathFromUssName(typeof(T).Name);
    }
}