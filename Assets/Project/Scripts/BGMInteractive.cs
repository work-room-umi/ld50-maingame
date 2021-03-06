using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class BGMInteractive : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField]
        public PlayerShip playerShip;

        [SerializeField]
        public DistanceMeasure distanceMeasure;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        void Update()
        {
            if (playerShip != null)
            {
                float value = 1 - playerShip.NormalizedHp();
                emitter.EventInstance.setParameterByName("interactive_bgm", value);
                float dist = 1 - distanceMeasure.NormalizedShipDistance();
                emitter.EventInstance.setParameterByName("battle_bgm", dist);
            }
        }
    }
}
