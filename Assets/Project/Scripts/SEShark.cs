using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SEShark : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField] public Attack attack;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            attack.OnAttacked += PlaySound;

        }

        void PlaySound()
        {
            emitter.Play();
        }
    }
}
