using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace umi.ld50{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        LevelSetting _levelSetting;

        [SerializeField]
        DrifterManager _drifterManager;

        [SerializeField]
        EnemyManager _enemyManager;

        [SerializeField]
        Attack _wavesAttack;
        // Start is called before the first frame update
        // async Task Start()
        // {
        //     await AsyncSelectLevelBundle();
        // }

        void Start(){
            AsyncSelectLevelBundle();
            AsyncIncrementWave();
        }

        async void AsyncIncrementWave(){
            while(true){
                _wavesAttack.SetAttackPower(_wavesAttack.AttackPower + _levelSetting.waveAttackStrengthInc);
                await Task.Delay((int)(1000f));
            }
        }

        async void AsyncSelectLevelBundle(){
            int bundleIndex = 0;
            while(bundleIndex<_levelSetting.levelBundles.Count){
                var bundle = _levelSetting.levelBundles[bundleIndex];
                _drifterManager.drifterSetting = bundle.drifterSetting;
                _enemyManager.enemyManagerValues = bundle.enemySetting;
                await Task.Delay((int)(bundle.duration*1000f));
                bundleIndex++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}