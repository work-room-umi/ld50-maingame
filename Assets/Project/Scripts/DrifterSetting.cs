using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50{
    [CreateAssetMenu(fileName = "DrifterSetting", menuName = "LevelDesign/DrifterSetting", order = 1)]
    public class DrifterSetting : ScriptableObject
    {
        public List<GameObjectWithWeight> prefabs;
        public int maxDrifterNum;
        [Tooltip("[s]")]
        public float spawnInterval;
    }
}