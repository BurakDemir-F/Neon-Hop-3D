using Game.Pool.AddressablesLocal.Scripts.Runtime;
using Game.Pool.AddressablesLocal.Scripts.VO;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Creators
{
    [CreateAssetMenu(menuName = "ScriptableData/Pool/PoolObjectCreator", fileName = "SimpleCreator", order = 0)]
    public class SimpleCreator : PoolObjectCreator
    {
        public override IPoolObjectController CreatePoolBehaviour(PoolConfig config, IPool pool, Transform root)
        {
            var poolCreature =  base.CreatePoolBehaviour(config, pool, root);
            poolCreature.Go.SetActive(false);
            return poolCreature;
        }
    }
}