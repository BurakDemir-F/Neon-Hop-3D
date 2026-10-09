using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public abstract class IDAttribute : DataAttribute
    {
        [SerializeField] protected int AlwaysAccept = -999;  
        public abstract bool IsMatching(int idToCheck);
        public abstract int GetId();
    }
}