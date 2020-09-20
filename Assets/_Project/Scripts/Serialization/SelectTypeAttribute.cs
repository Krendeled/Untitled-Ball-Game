﻿using System;
using UnityEngine;

namespace UntitledBallGame.Serialization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectTypeAttribute : PropertyAttribute
    {
        public Type FieldType { get; }
        public Representation Representation { get; }

        public SelectTypeAttribute(Type fieldType, Representation representation = Representation.Name)
        {
            FieldType = fieldType;
            Representation = representation;
        }
    }
}