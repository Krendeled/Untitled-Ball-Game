using System;
using System.Collections.Generic;

namespace UntitledBallGame.Serialization
{
    public static class CachedTypes
    {
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        public static Type GetType(string typeName)
        {
            if (TypeCache.TryGetValue(typeName, out Type type)) return type;

            type = !string.IsNullOrEmpty(typeName) ? Type.GetType(typeName) : null;
            TypeCache[typeName] = type;
            return type;
        }
    }
}