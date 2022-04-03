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

        //TODO playerShip.CurrentTiltingRateを実装してもらったらコメント解除
        void Update()
        {
            if (playerShip != null)
            {
                //float value = playerShip.CurrentTiltingRate();
                //emitter.EventInstance.setParameterByName("creak_sound", value);
            }
        }
    }
}
