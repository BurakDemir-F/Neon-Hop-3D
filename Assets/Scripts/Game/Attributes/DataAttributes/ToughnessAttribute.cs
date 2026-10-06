using UnityEngine;

[System.Serializable]
public class ToughnessAttribute : DataAttribute
{
    [SerializeField] private int _hitCount;

    public ToughnessAttribute()
    {
    }

    public ToughnessAttribute(int hitCount)
    {
        _hitCount = hitCount;
    }

    public int HitCount
    {
        get => _hitCount;
        set => _hitCount = value;
    }
}

