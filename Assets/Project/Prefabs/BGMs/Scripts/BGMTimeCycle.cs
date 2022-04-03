using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class BGMTimeCycle : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        private float elapsedTime;
        private float sinOffset = Mathf.PI / 2;

        [SerializeField]
        public float timeCycle = 60.0f;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        public void Start()
        {
            elapsedTime = 0;
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;
            float Hz = 1.0f / timeCycle;
            float value = Mathf.Sin(2 * Mathf.PI * Hz * elapsedTime - sinOffset) / 2 + 0.5f;
            emitter.EventInstance.setParameterByName("time_cycle", value);
            Debug.Log(value);
        }
    }
}
