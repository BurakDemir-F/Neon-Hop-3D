using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class CrackAttribute : WorkerAttribute<StackableItem>
    {
        public override IEnumerator DoAttributeWork(StackableItem owner, WorkChecker workChecker)
        {
            if (owner != null)
            {
                yield return DG.Tweening.ShortcutExtensions.DOScale(owner.transform, UnityEngine.Vector3.zero, 0.15f).WaitForCompletion();
            }
        }
    }
}