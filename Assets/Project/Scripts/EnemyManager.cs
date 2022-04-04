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
        private EnemyManagerScriptableObject enemyManagerValues;

        [SerializeField] private float enemyNormalizedMaxDistance = 25f;
        private List<GameObject> enemies = new List<GameObject>();
        public float NormalizedShipDistance()
        {
            enemies = enemies.Where(e => e != null).ToList();
            var  nearestDist = enemies.Select(e => Vector3.Distance(e.transform.position, _ship.transform.position)).Min();
            var normalizedDist = Mathf.Clamp01(nearestDist / enemyNormalizedMaxDistance);
            return normalizedDist;
        }

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
            while(true){
                if (enemyManagerValues == null) return; 
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
            GameObject prefab = enemyManagerValues.prefabs[index];
            GameObject enemy = Instantiate (prefab, spawnPosition, Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            enemy.transform.LookAt(_ship.gameObject.transform);
            enemy.layer = LayerMask.NameToLayer("Obstacle");
            enemies.Add(enemy);
            
            // Sharklocomoterにshipを追従させる
            SharkLocomoter sharkLocomoter = enemy.GetComponent<SharkLocomoter>();
            sharkLocomoter.SetTarget(_ship.gameObject.transform);
        }
    }
}
