using General;
using UnityEngine;

public abstract class AttributeBase : MonoBehaviour
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