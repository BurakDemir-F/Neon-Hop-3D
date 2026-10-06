using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class MovementAttribute : DataAttribute
{
    [SerializeField] private int _moveDuration;
    [SerializeField] private Ease _ease;

    public int MoveDuration => _moveDuration;

    public Ease Ease => _ease;
}