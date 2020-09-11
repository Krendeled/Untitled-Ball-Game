using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UntitledBallGame.Serialization
{
	[Serializable]
	public class ClassTypeReference : ISerializationCallbackReceiver
	{
#if UNITY_EDITOR
		public const string NoneElement = "(None)";
		[SerializeField] private Type _type;
#endif
		
		[SerializeField] private string _serializedType;

		public ClassTypeReference()
		{
		}
	
		public ClassTypeReference(Type type)
		{
			_type = type;
		}

		public void OnBeforeSerialize()
		{
			_serializedType = _type == null ? NoneElement : _type.AssemblyQualifiedName;
		} 

		public void OnAfterDeserialize()
		{
			_type = CachedTypes.GetType(_serializedType);
		} 
	
		public static implicit operator Type(ClassTypeReference typeReference)
		{
			return typeReference._type;
		}

		public static implicit operator ClassTypeReference(Type type)
		{
			return new ClassTypeReference(type);
		}

		public override string ToString()
		{
			return _type == null ? NoneElement : _type.AssemblyQualifiedName;
		}
	}
}