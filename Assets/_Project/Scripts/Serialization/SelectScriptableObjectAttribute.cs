using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectScriptableObjectAttribute : PropertyAttribute
    {
        public Type ScriptableType { get; }
        
        public SelectScriptableObjectAttribute(Type scriptableType)
        {
            ScriptableType = scriptableType;
        }
    }
}