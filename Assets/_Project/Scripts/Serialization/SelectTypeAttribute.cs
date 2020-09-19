using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectTypeAttribute : PropertyAttribute
    {
        public Type FieldType { get; }

        public SelectTypeAttribute(Type fieldType)
        {
            FieldType = fieldType;
        }
    }
}