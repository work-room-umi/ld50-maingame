using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SEConstruction : MonoBehaviour
    {
        private StudioEventEmitter emitter;

        [SerializeField] public SETrigger seTrigger;

        [SerializeField] public float delayTime = 6f;

        [SerializeField] public float coolTime = 10f;

        private float coolDown = 0;
        private bool isRady => (coolDown <= 0);

        public void Awake()
        {
            emitter = GetComponent<StudioEventEmitter>();
        }

        // Start is called before the first frame update
        void Start()
        {
            seTrigger.OnSoundPlay += PlaySound;

        }
        private void Update()
        {
            if (!isRady)
            {
                coolDown -= Time.deltaTime;
            }
        }

        void PlaySound()
        {
            if (isRady)
            {
                StartCoroutine(DelayAction(delayTime));

            }
        }
        IEnumerator DelayAction(float delayTime)
        {
            coolDown = coolTime;
            yield return new WaitForSeconds(delayTime);
            emitter.Play();
        }


    }
}
