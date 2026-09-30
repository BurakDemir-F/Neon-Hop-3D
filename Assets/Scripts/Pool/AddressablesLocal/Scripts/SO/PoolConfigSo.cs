using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.VO;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.SO
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "ScriptableData/Pool/PoolConfig", order = 0)]
    public class PoolConfigSo : ScriptableObject
    {
        public List<PoolConfig> PoolConfig;
    }
}