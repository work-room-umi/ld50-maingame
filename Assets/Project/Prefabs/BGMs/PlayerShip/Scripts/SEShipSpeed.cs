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
        public PlayerShip playerShip;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        //TODO �v���p�e�B�������Ă��������R�����g����
        //void Update()
        //{
        //    if (playerShip != null)
        //    {
        //        float value = playerShip.Speed;
        //        emitter.EventInstance.setParameterByName("ship_speed", value);
        //    }
        //}
    }
}
