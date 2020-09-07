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
			if (_type == null || string.IsNullOrEmpty(_type.FullName))
				_serializedType = NoneElement;
			else
				_serializedType = _type.FullName;
		} 

		public void OnAfterDeserialize()
		{
			_type = CachedTypes.GetType(_serializedType);
		} 
	
		public static implicit operator Type(ClassTypeReference testType)
		{
			return testType._type;
		}

		public static implicit operator ClassTypeReference(Type type)
		{
			return new ClassTypeReference(type);
		}

		public override string ToString()
		{
			return _type == null ? NoneElement : _type.FullName;
		}
	}
}