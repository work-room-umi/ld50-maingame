using umi.ld50;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace umi.ld50 {
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _ship;
        [SerializeField]
        public EnemyManagerScriptableObject enemyManagerValues;

        // Start is called before the first frame update
        void Start()
        {
            
            AsyncSpawnWithInterval();
        }

        // Update is called once per frame
        void Update()
        {
            if (_ship == null){
                _ship = FindObjectOfType<PlayerShip>();
            }
        }

        async void AsyncSpawnWithInterval(){
            await Task.Delay((int)(enemyManagerValues.firstSpawnDeley*1000f));
            while(true){
                if (this.transform.childCount < enemyManagerValues.numberOfPrefabsToCreate)
                {
                    SpawnEnemy();
                }
                await Task.Delay((int)(enemyManagerValues.spawnInterval*1000f));
            }
        }

        Vector3 PointOncircle(in float angle, in float radius){
            var dirRotated = Quaternion.AngleAxis(angle, Vector3.up)*Vector3.forward;
            return radius * dirRotated;
        }
        GameObject SelectSpawnObjectRandomly(in EnemyManagerScriptableObject setting){
            int index = GetRandomWeightedIndex(setting.prefabs.Select(pair => pair.weight).ToArray());
            return setting.prefabs[index].prefab;
        }

        // cf. https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
        int GetRandomWeightedIndex(float[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;
 
            float w;
            float t = 0;
            int i;
            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];
 
                if (float.IsPositiveInfinity(w))
                {
                    return i;
                }
                else if (w >= 0f && !float.IsNaN(w))
                {
                    t += weights[i];
                }
            }
 
            float r = UnityEngine.Random.value;
            float s = 0f;
 
            for (i = 0; i < weights.Length; i++)
            {
                w = weights[i];
                if (float.IsNaN(w) || w <= 0f) continue;
 
                s += w / t;
                if (s >= r) return i;
            }
 
            return -1;
        }

        void SpawnEnemy()
        {
            if (_ship == null) return;
            // スポーン位置を計算
            var shipPosition = _ship.gameObject.transform.position;
            var spawnZoneCenter = shipPosition + _ship.gameObject.transform.forward * enemyManagerValues.spawnDist;
            var spawnPosition = spawnZoneCenter + PointOncircle(UnityEngine.Random.Range(-180f,180f), UnityEngine.Random.Range(0f, enemyManagerValues.spawnDist));

            // コライダーと干渉している場合スポーンのpositionをずらす
            //上から下にSphereCast
            Ray ray = new Ray(spawnPosition + Vector3.up*20f, Vector3.down);
            int layerMask = LayerMask.GetMask("Obstacle");
            Physics.queriesHitBackfaces = true;
            var hits = new RaycastHit[1];            
            var hitNum = Physics.SphereCastNonAlloc(ray, 0.5f, hits, 10f, layerMask);
            if (hitNum != 0)
            {
                var col = hits[0].collider;
                var bounds = col.bounds;
                spawnPosition += new Vector3(bounds.max.x, 0, bounds.max.z);    
            }

            int index = UnityEngine.Random.Range(0, enemyManagerValues.prefabs.Count);
            // GameObject prefab = enemyManagerValues.prefabs[index].prefab;
            GameObject prefab = SelectSpawnObjectRandomly(enemyManagerValues);
            GameObject enemy = Instantiate (prefab, spawnPosition, Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            enemy.transform.LookAt(_ship.gameObject.transform);
            enemy.layer = LayerMask.NameToLayer("Obstacle");
            // Sharklocomoterにshipを追従させる
            SharkLocomoter sharkLocomoter = enemy.GetComponent<SharkLocomoter>();
            sharkLocomoter.SetTarget(_ship.gameObject.transform);
        }
    }
}
