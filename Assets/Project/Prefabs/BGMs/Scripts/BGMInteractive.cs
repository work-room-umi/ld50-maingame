using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class BGMInteractive : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        //TODO PlayerShip���R�~�b�g���ꂽ��A�R�����g������
        //[SerializeField]
        //public PlayerShip playerShip;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        void Update()
        {
            //if (playerShip != null)
            //{
            //    float value = playerShip.NormalizedHp;
            //    emitter.EventInstance.setParameterByName("interactive_bgm", value);
            //}
        }
    }
}
