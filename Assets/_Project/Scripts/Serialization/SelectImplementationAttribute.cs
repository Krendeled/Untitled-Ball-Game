using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectImplementationAttribute : PropertyAttribute
    {
        public Type FieldType { get; }

        public SelectImplementationAttribute(Type fieldType)
        {
            FieldType = fieldType;
        }
    }
}