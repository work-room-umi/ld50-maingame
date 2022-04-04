using umi.ld50;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

namespace umi.ld50 {
    public class DrifterManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _ship;
        [SerializeField]
        private float _deleteDist;
        [SerializeField]
        private float _spawnDist;
        // [SerializeField]
        // private int _maxDrifterNum;
        [SerializeField]
        private List<GameObject> _drifterPrefabs;
        [SerializeField]
        float _spawnZoneCenterRadius;

        [SerializeField]
        public DrifterSetting drifterSetting;

        // Start is called before the first frame update
        private void Start()
        {
            if (_ship==null)
                _ship =  (PlayerShip)FindObjectOfType(typeof(PlayerShip));
            AsyncSpawnWithInterval();
        }

        async void AsyncSpawnWithInterval(){
            while(true){
                if(this == null) return;
                if (this.transform.childCount < drifterSetting.maxDrifterNum)
                {
                    Spawn();
                }
                await Task.Delay((int)(drifterSetting.spawnInterval*1000f));
            }
        }

        private Vector3 GenerateRandomBasePosition()
        {
            float generateComponent()
            {
                float c = UnityEngine.Random.Range(_spawnDist, _deleteDist);
                if (UnityEngine.Random.value < .5)
                {
                    return c;
                }
                else
                {
                    return -c;
                }
            };
            return new Vector3(generateComponent(), 0, generateComponent());
        }

        Vector3 SpawnPosition(){
            var randomAngle = UnityEngine.Random.Range(-180f,180f);
            var shipPositionDir = _ship.transform.position.normalized;
            var spawnZoneCenter = -shipPositionDir * _spawnZoneCenterRadius;
            var spawnPointOnCircleZone = spawnZoneCenter + PointOncircle(UnityEngine.Random.Range(-180f,180f), UnityEngine.Random.Range(0f,_spawnZoneCenterRadius));
            return AdjustedSpawnPosition(spawnPointOnCircleZone);
        }

        Vector3 PointOncircle(in float angle, in float radius){
            var dirRotated = Quaternion.AngleAxis(angle, Vector3.up)*Vector3.forward;
            return radius * dirRotated;
        }

        GameObject SelectSpawnObjectRandomly(DrifterSetting setting){
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

        void GC(in Vector3 shipPosition)
        {
            //削除対象を集める(for/foreachの中で要素を削除するとエラーで怒られたり、要素数がずれたりするので、2周に分ける)
            List<Transform> removingObjects = new List<Transform>();
            foreach(Transform drifter in gameObject.transform){
                Vector3 position = drifter.transform.position;
                float dist = Vector3.Distance(position, shipPosition);
                if (_deleteDist < dist) removingObjects.Add(drifter);
            }
            //削除
            foreach (var drifter in removingObjects)
                Destroy(drifter.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // Vector3 shipPosition = _ship.gameObject.transform.position;
            // GC(shipPosition);
            
            // if (this.transform.childCount<_maxDrifterNum)
            // {
            //     Spawn();
            // }
        }

        void Spawn(){
            // 漂流物の初期位置を計算する
            Vector3 shipPosition = _ship.gameObject.transform.position;
            // Vector3 basePosition = GenerateRandomBasePosition();
            // Vector3 basePosition = SpawnPosition();
            // Vector3 position = SpawnPosition();

            int index = UnityEngine.Random.Range(0, _drifterPrefabs.Count);
            // GameObject prefab = _drifterPrefabs[index];
            GameObject prefab = SelectSpawnObjectRandomly(drifterSetting);
            var randomAngle = UnityEngine.Random.Range(-180f,180f);
            var randomOrientation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            var randomScale = UnityEngine.Random.Range(-1,2.5f);
            var pos = SpawnPosition();
            GameObject driftObj = Instantiate (prefab, SpawnPosition(), Quaternion.identity);
            var destroyer = driftObj.AddComponent<DeleyTimeDestroyer>();
            destroyer.time = 120;
            driftObj.transform.rotation   = randomOrientation;
            driftObj.transform.position   = pos;
            // スケールは可変のオブジェクトとそうでないものがあるのでPrefab側でバリエーションを設定した方が良さそうです
            // driftObj.transform.localScale = dirft*randomScale;
            driftObj.transform.parent = gameObject.transform;
        }

         Vector3 AdjustedSpawnPosition(in Vector3 spawnPosition){
             //上から下にSphereCast
             Ray ray = new Ray(spawnPosition + Vector3.up*20f, Vector3.down);
             int layerMask = LayerMask.GetMask("Obstacle");
             Physics.queriesHitBackfaces = true;
             var resultPosition = spawnPosition;
             var hits = new RaycastHit[1];            
             var hitNum = Physics.SphereCastNonAlloc(ray, 0.5f, hits, 10f, layerMask);
             if (hitNum == 0) return resultPosition;
             var col = hits[0].collider;
             var bounds = col.bounds;
             resultPosition += new Vector3(bounds.max.x, 0, bounds.max.z);
             return resultPosition;
        }
    }

}
