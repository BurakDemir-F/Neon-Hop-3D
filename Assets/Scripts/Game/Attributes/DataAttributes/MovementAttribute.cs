using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class MovementAttribute : DataAttribute
{
    [SerializeField] private float _moveDuration = 0.35f;
    [SerializeField] private Ease _ease = Ease.OutQuad;

    public float MoveDuration => _moveDuration;

    public Ease Ease => _ease;
}