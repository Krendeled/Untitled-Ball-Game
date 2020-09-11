using System;
using System.Collections.Generic;
using System.Linq;

namespace UntitledBallGame.Utility
{
    public static class ReflectionUtility
    {
        public static List<Type> GetImplementations(Type type, Predicate<Type> predicate = null)
        {
            if (predicate == null)
                predicate = t => true;
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && predicate(t))
                .OrderBy(t => t.FullName)
                .ToList();
        }
    }
}