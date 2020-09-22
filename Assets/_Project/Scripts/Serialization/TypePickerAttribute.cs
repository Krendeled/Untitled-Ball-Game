using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TypePickerAttribute : PropertyAttribute
    {
        public Type FieldType { get; }
        public Representation Representation { get; }

        public TypePickerAttribute(Type fieldType, Representation representation = Representation.Name)
        {
            FieldType = fieldType;
            Representation = representation;
        }
    }
}