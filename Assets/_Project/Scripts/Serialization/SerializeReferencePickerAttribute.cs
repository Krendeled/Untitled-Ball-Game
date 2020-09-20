using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeReferencePickerAttribute : PropertyAttribute
    {
        public Representation Representation { get; }
        
        public SerializeReferencePickerAttribute(Representation representation = Representation.Name)
        {
            Representation = representation;
        }
    }
}