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
        public PlayerShipLocomoter playerShipLocomoter;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        void Update()
        {
            if (playerShipLocomoter != null)
            {
                float value = playerShipLocomoter.CurrentTiltingRate();
                emitter.EventInstance.setParameterByName("creak_sound", value);
            }
        }
    }
}
