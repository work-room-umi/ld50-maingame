using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SEBreak : MonoBehaviour
    {
        private StudioEventEmitter emitter;


        [SerializeField]
        public PlayerShip playerShip;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
            playerShip.BreakPartsAction += PlaySound;
        }

        void PlaySound()
        {
            emitter.Play();
        }
    }
}
