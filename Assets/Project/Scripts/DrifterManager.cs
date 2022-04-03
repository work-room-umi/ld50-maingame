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

        private List<GameObject> managedDrifters;

        // Start is called before the first frame update
        private void Start()
        {
            managedDrifters = new List<GameObject>();
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

        void GC(Vector3 shipPosition)
        {
            //削除対象を集める(for/foreachの中で要素を削除するとエラーで怒られたり、要素数がずれたりするので、2周に分ける)
            List<GameObject> removingObjects = new List<GameObject>();
            foreach (var drifter in managedDrifters)
            {
                Vector3 position = drifter.transform.position;
                float dist = Vector3.Distance(position, shipPosition);
                if (deleteDist < dist) removingObjects.Add(drifter);
            }
            //削除
            foreach (var drifter in removingObjects)
            {
                managedDrifters.Remove(drifter);
                Destroy(drifter);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 shipPosition = ship.transform.position;
            GC(shipPosition);
            
            if (managedDrifters.Count<maxDrifterNum)
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

                managedDrifters.Add(drifter);
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
