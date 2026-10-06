using System.Collections;
using Game.Sorcerum;
using General;
using UnityEngine;

[System.Serializable]
public abstract class AttributeBase
{
    protected IContextProvider _context;
    
    public void Initialize(IContextProvider context)
    {
        _context = context;
        OnAfterInitialize();
    }

    protected virtual void OnAfterInitialize()
    {
        
    }

}

public class WorkChecker
{
    public bool ShouldWork { get; } = true;
}

[System.Serializable]
public abstract class WorkerAttribute<T> : AttributeBase
{
    public abstract IEnumerator DoAttributeWork(T owner, WorkChecker workChecker);
}

[System.Serializable]
public class DataAttribute : AttributeBase
{
    
}

public class RuntimeAttribute
{
    
}