using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class BallVisualAttribute : DataAttribute
    {
        [SerializeField] private Sprite _ballVisual;

        public Sprite BallVisual => _ballVisual;
    }
}