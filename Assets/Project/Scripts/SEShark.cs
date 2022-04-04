using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SEShark : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField] public SETrigger seTrigger;

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            seTrigger.OnSoundPlay += PlaySound;

        }

        void PlaySound()
        {
            emitter.Play();
        }
    }
}
