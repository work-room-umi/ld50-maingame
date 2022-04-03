using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50 {
    public class DrifterManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject ship;
        [SerializeField]
        private float deleteDist;
        [SerializeField]
        private float spawnDist;
        [SerializeField]
        private int maxDrifterNum;
        [SerializeField]
        private List<GameObject> drifterPrefabs;
        [SerializeField]
        private DriftParameter driftParameter;

        // Start is called before the first frame update
        private void Start()
        {
        }

        private Vector3 GenerateRandomBasePosition()
        {
            float generateComponent()
            {
                float c = UnityEngine.Random.Range(spawnDist, deleteDist);
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

        // Update is called once per frame
        void Update()
        {
            Vector3 shipPosition = ship.transform.position;
            
            if (this.transform.childCount<maxDrifterNum)
            {
                // 漂流物の初期位置を計算する
                Vector3 basePosition = GenerateRandomBasePosition();
                Vector3 position = shipPosition + basePosition;

                int index = UnityEngine.Random.Range(0, drifterPrefabs.Count);
                GameObject prefab = drifterPrefabs[index];
                GameObject drifter = Instantiate (prefab, position, Quaternion.identity);
                drifter.transform.parent = gameObject.transform;

                Drifter _drifter = drifter.AddComponent<Drifter>();
                _drifter._freq = driftParameter.freq;
                _drifter._amp = driftParameter.amp;
                _drifter._wave = driftParameter.wave;
            }
        }

        [Serializable]
        public struct DriftParameter
        {
            public float freq;
            public float amp;
            public GameObject wave;
        }
    }
}
