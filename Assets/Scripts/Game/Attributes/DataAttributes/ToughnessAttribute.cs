
using UnityEngine;

[System.Serializable]
public class ToughnessAttribute : DataAttribute
{
    [SerializeField] private int _hitCount;

    public int HitCount => _hitCount;
}