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

public ref struct AttributeWork
{
    public bool RequireTween;
}

public abstract class WorkerAttribute : AttributeBase
{
    public abstract AttributeWork DoAttributeWork();
}

public class DataAttribute : AttributeBase
{
    
}