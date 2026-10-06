using System.Collections;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class CrackAttribute : WorkerAttribute<StackableItem>
    {
        public override IEnumerator DoAttributeWork(StackableItem owner, WorkChecker workChecker)
        {
            throw new System.NotImplementedException();
        }
    }
}