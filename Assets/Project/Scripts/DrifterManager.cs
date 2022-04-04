using umi.ld50;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50 {
    public class DrifterManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _ship;
        [SerializeField]
        private float _deleteDist;
        [SerializeField]
        private float _spawnDist;
        [SerializeField]
        private int _maxDrifterNum;
        [SerializeField]
        private List<GameObject> _drifterPrefabs;
        [SerializeField]
        float _spawnZoneCenterRadius;

        // Start is called before the first frame update
        private void Start()
        {
            if (_ship==null)
                _ship =  (PlayerShip)FindObjectOfType(typeof(PlayerShip));
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
            return spawnPointOnCircleZone;
        }

        Vector3 PointOncircle(in float angle, in float radius){
            var dirRotated = Quaternion.AngleAxis(angle, Vector3.up)*Vector3.forward;
            return radius * dirRotated;
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
            Vector3 shipPosition = _ship.gameObject.transform.position;
            // GC(shipPosition);
            
            if (this.transform.childCount<_maxDrifterNum)
            {
                Spawn();
            }
        }

        void Spawn(){
            // 漂流物の初期位置を計算する
            Vector3 shipPosition = _ship.gameObject.transform.position;
            // Vector3 basePosition = GenerateRandomBasePosition();
            // Vector3 basePosition = SpawnPosition();
            // Vector3 position = SpawnPosition();

            int index = UnityEngine.Random.Range(0, _drifterPrefabs.Count);
            GameObject prefab = _drifterPrefabs[index];

            // TODO ランダムで大きさ&向きを変える
            var randomAngle = UnityEngine.Random.Range(-180f,180f);
            var randomOrientation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            var randomScale = UnityEngine.Random.Range(-1,2.5f);
            GameObject driftObj = Instantiate (prefab, SpawnPosition(), Quaternion.identity);
            driftObj.transform.rotation   = randomOrientation;
            driftObj.transform.position   = SpawnPosition();
            // スケールは可変のオブジェクトとそうでないものがあるのでPrefab側でバリエーションを設定した方が良さそうです
            // driftObj.transform.localScale = dirft*randomScale;
            driftObj.transform.parent = gameObject.transform;
        }
    }
}
