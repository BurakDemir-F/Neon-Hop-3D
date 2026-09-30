using Game.Pool.AddressablesLocal.Scripts.Runtime;
using Game.Pool.AddressablesLocal.Scripts.VO;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Creators
{
    public class MazeWorldCreator : PoolObjectCreator
    {
        public override IPoolObject CreatePoolBehaviour(PoolConfig config, IPool pool, Transform root)
        {
            var poolCreature =  base.CreatePoolBehaviour(config, pool, root);
            poolCreature.Go.SetActive(false);
            return poolCreature;
        }
    }
}