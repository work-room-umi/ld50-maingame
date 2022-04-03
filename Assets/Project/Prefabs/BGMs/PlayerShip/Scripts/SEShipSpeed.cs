using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SEShipSpeed : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField]
        public PlayerShipLocomoter playerShipLocomoter;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        //TODO playerShipLocomoter.CurrentSpeedRateを実装してもらったらコメント解除
        void Update()
        {
            if (playerShipLocomoter != null)
            {
                //float value = playerShipLocomoter.CurrentSpeedRate();
                //emitter.EventInstance.setParameterByName("ship_speed", value);
            }
        }
    }
}
