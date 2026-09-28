using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackableItem : MonoBehaviour, IPoolObject
{
    protected Coroutine _crackCor;

    public virtual void Crack()
    {
        
    }

    public virtual IEnumerator CrackCor()
    {
        yield return null;
    }

    public void StopCrack()
    {
        if(_crackCor != null)
            StopCoroutine(_crackCor);
    }
}

public abstract class BreakerBall
{
    
}

public class ColorBall
{
    public Color Color { get; }

    public virtual void Jump()
    {
        
    }
}

public abstract class AttributeBase
{
    protected IContext _context;
    
    public void Initialize(IContext context)
    {
        _context = context;
        OnAfterInitialize();
    }

    protected virtual void OnAfterInitialize()
    {
        
    }
}

public abstract class CrackAttribute : AttributeBase
{
    
}

public class JumperAttribute : AttributeBase
{
    
}

public interface IContext
{
    
}

public interface Context : IContext{}

public interface IPoolObject
{
    
}

public class AttributeCollection : MonoBehaviour
{
    private List<AttributeBase> _attributes = new();

    public void SetAttributes(List<AttributeBase> attributeBaseList)
    {
        _attributes.Clear();
        foreach (var attributeBase in attributeBaseList)
        {
            _attributes.Add(attributeBase);
        }
    }

    private void SelfFill()
    {
        _attributes.Clear();
        GetComponentsInChildren<AttributeBase>(_attributes);
    }
    
    public void Initialize(IContext context)
    {
        foreach (var attributeBase in _attributes)
        {
            attributeBase.Initialize(context);
        }
    }
}
