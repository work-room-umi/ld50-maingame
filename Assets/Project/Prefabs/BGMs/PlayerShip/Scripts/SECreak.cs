using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SECreak : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField]
        public PlayerShip playerShip;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        void Update()
        {
            if (playerShip != null)
            {
                float value = 1 - playerShip.NormalizedHp;
                emitter.EventInstance.setParameterByName("creak_sound", value);
            }
        }
    }
}
