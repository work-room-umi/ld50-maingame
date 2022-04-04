
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    [CreateAssetMenu(fileName = "LevelSetting", menuName = "LevelDesign/LevelSetting", order = 1)]
    public class LevelSetting : ScriptableObject
    {
        public List<LevelBundle> levelBundles;
    }
}