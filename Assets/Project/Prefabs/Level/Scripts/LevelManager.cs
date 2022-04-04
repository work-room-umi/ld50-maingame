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
        // Start is called before the first frame update
        // async Task Start()
        // {
        //     await AsyncSelectLevelBundle();
        // }

        void Start(){
            AsyncSelectLevelBundle();
        }


        async void AsyncSelectLevelBundle(){
            int bundleIndex = 0;
            while(bundleIndex<_levelSetting.levelBundles.Count){
                Debug.Log(1);
                var bundle = _levelSetting.levelBundles[bundleIndex];
                _drifterManager.drifterSetting = bundle.drifterSetting;
                _enemyManager.enemyManagerValues = bundle.enemySetting;
                Debug.Log(2);
                await Task.Delay((int)(bundle.duration*1000f));
                Debug.Log(3);
                bundleIndex++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}