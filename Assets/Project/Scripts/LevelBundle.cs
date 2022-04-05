using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50{
    [System.Serializable] 
    public struct LevelBundle{
        public EnemyManagerScriptableObject enemySetting;
        public DrifterSetting drifterSetting;
        public float duration;
    }
}