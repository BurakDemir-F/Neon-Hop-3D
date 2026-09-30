using System.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Game.Sorcerum
{

    [InitializeOnLoad]
    public static class ImplementationCacheManager
    {
        private static readonly Dictionary<Type, List<Type>> ImplementationsCache = new();
        private static readonly Dictionary<Type, List<string>> ImplementationNameCache = new();
        private static readonly Dictionary<string, Type> NameTypeDict = new();

        public static IReadOnlyDictionary<Type, List<Type>> Implementations => ImplementationsCache;
        public static IReadOnlyDictionary<Type, List<string>> ImplementationNames => ImplementationNameCache;
        public static IReadOnlyDictionary<string, Type> NameTypes => NameTypeDict;
        
        static ImplementationCacheManager()
        {
            ImplementationsCache.Clear();
            ImplementationNameCache.Clear();
            NameTypeDict.Clear();
        }

        public static void FindAllImplementationsFor(Type baseType)
        {
            if(baseType == null)
                return;
            
            var typeCache = ImplementationCacheManager.Implementations;
            
            if(typeCache.ContainsKey(baseType))
                return;
        
            if (baseType.IsArray) baseType = baseType.GetElementType();
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(List<>))
            {
                baseType = baseType.GetGenericArguments()[0];
            }

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);
        
            var typeList = types.ToList();
            ImplementationCacheManager.AddImplementations(baseType, typeList);
        }

        public static void AddImplementations(Type type, List<Type> implementations)
        {
            if (ImplementationsCache.TryAdd(type, implementations))
            {
                var nameList = implementations.Select(implementation => implementation.Name).ToList();
                //todo:: when sorted problem happens look for later.
                // nameList.Sort();
                
                ImplementationNameCache.Add(type, nameList);
                implementations.ForEach(implementation => NameTypeDict.Add(implementation.Name, implementation));
            }
        }
    }
}