using System;
using System.Collections.Generic;

namespace UntitledBallGame.Serialization
{
    public static class CachedTypes
    {
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName)) return null;
            
            if (TypeCache.TryGetValue(typeName, out Type type)) return type;

            type = Type.GetType(typeName);
            if (type == null) return null;
            
            TypeCache[typeName] = type;
            return type;
        }
    }
}