using UnityEngine;

namespace Game.Shared.AddressablesLocal.Scripts.Utilities
{
    public static class UnityEngineObjectExtensions
    {
        public static bool IsNotNull(this Object obj)
        {
            return obj != null;
        }

        public static T GetComponentFromAllTransform<T>(this GameObject obj, T defaultValue)
        {
            var component = obj.GetComponent<T>();
            if (component != null) return component;

            component = obj.GetComponentInParent<T>();
            if (component != null) return component;

            component = obj.GetComponentInChildren<T>();
            if (component != null) return component;

            return defaultValue;
        }

        public static void EnsureComponentExists<T>(this GameObject obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
                obj.AddComponent<T>();
        }

        public static T GetComponentEnsure<T>(this GameObject obj) where T : Component
        {
            return obj.TryGetComponent<T>(out var component) ? component : obj.AddComponent<T>();
        }

        public static T GetComponentEnsure<T>(this GameObject obj, GameObject componentObject, Transform parent)
        {
            return obj.TryGetComponent<T>(out var component)
                ? component
                : Object.Instantiate(componentObject, parent).GetComponent<T>();
        }

        public static T GetComponentEnsureInChildren<T>(this GameObject obj, GameObject componentObject,
            Transform parent)
        {
            return obj.TryGetComponentInChildren<T>(out var component)
                ? component
                : Object.Instantiate(componentObject, parent).GetComponentInChildren<T>();
        }

        public static bool TryGetComponentInChildren<T>(this GameObject obj, out T component)
        {
            component = obj.GetComponentInChildren<T>();
            return component != null;
        }

        
        
        public static T GetFirstComponentInChildrenOrSelf<T>(this GameObject obj)
        {
            if (obj.TryGetComponent<T>(out var component))
            {
                return component;
            }
            
            var transform = obj.transform;
            
            for(var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<T>(out component))
                {
                    return component;   
                }
            }
            
            return default;
        }

        public static bool IsSceneObject(this GameObject go)
        {
            return go.scene.name != null;
        }

        public static void Print(this object obj)
        {
            if (obj == null)
            {
                Debug.Log("object is null");
                return;
            }

            Debug.Log(obj);
        }

        public static void Print(this object obj, GameObject go)
        {
            if (obj == null)
            {
                Debug.Log("object is null");
                return;
            }

            Debug.Log(obj,go);
        }

        public static void PrintAssert(this object obj, bool assertion)
        {
            Debug.Assert(assertion, obj);
        }
        
        public static bool SafeUnityEquals(object a, object b) 
        {
            if (ReferenceEquals(a, b)) return true;
            
            if (a is UnityEngine.Object objA && objA == null) return b is UnityEngine.Object objB && objB == null;
            if (b is UnityEngine.Object objC && objC == null) return false;

            return a != null && a.Equals(b);
        }
        
    }
}