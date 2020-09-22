using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ScriptableObjectPickerAttribute : PropertyAttribute
    {
        public Type ScriptableType { get; }
        
        public ScriptableObjectPickerAttribute(Type scriptableType)
        {
            ScriptableType = scriptableType;
        }
    }
}