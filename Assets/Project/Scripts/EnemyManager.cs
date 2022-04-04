using umi.ld50;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50 {
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _ship;
        EnemyManagerScriptableObject enemyManagerValues;
        int instanceNumber = 1;

        // Start is called before the first frame update
        void Start()
        {
            if (_ship==null)
                _ship =  (PlayerShip)FindObjectOfType(typeof(PlayerShip));
        }

        // Update is called once per frame
        void Update()
        {
            SpawnEnemy();
        }


        void SpawnEnemy()
        {
                // 原点からの距離が一定の場合のみスポーン
                Vector3 shipPosition = _ship.gameObject.transform.position;
                float distanceFromOrigin = Vector3.Distance(shipPosition, new Vector3(0, 0, 0));
                if (distanceFromOrigin > enemyManagerValues.spawnDist && instanceNumber < enemyManagerValues.numberOfPrefabsToCreate){
                    int index = UnityEngine.Random.Range(0, enemyManagerValues.prefabs.Count);
                    GameObject prefab = enemyManagerValues.prefabs[index];

                    // スポーン位置を計算
                    Vector3 spanwPosition = new Vector3(0, 0, 0) - shipPosition;
                    GameObject driftObj = Instantiate (prefab, spanwPosition, Quaternion.identity);
                }
        }
    }
}
