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
        private DriftParameter _driftParameter;

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

        void GC(Vector3 shipPosition)
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
            GC(shipPosition);
            
            if (this.transform.childCount<_maxDrifterNum)
            {
                // 漂流物の初期位置を計算する
                Vector3 basePosition = GenerateRandomBasePosition();
                Vector3 position = shipPosition + basePosition;

                int index = UnityEngine.Random.Range(0, _drifterPrefabs.Count);
                GameObject prefab = _drifterPrefabs[index];
                // TODO ランダムで大きさ&向きを変える
                GameObject driftObj = Instantiate (prefab, position, Quaternion.identity);
                driftObj.transform.parent = gameObject.transform;

                var drifter = driftObj.AddComponent<Drifter>();
                drifter._freq = _driftParameter._freq;
                drifter._amp = _driftParameter._amp;
                drifter._noiseMoveAmp = _driftParameter._noiseMoveAmp;
                drifter._noiseScale = _driftParameter._noiseScale;
                drifter._noiseTimeScale = _driftParameter._noiseTimeScale;
                drifter._wave = _driftParameter._wave==null? ((Waves)FindObjectOfType(typeof(Waves))).gameObject : _driftParameter._wave;
            }
        }

        [Serializable]
        public struct DriftParameter
        {
            public float _freq;
            public float _amp;
            public float _noiseMoveAmp;
            public float _noiseScale;
            public float _noiseTimeScale;
            public GameObject _wave;
        }
    }
}
