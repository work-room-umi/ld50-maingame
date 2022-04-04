using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class SETrigger : MonoBehaviour
    {
        public Action OnSoundPlay;

        private void OnTriggerEnter(Collider other)
        {
            OnSoundPlay?.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnSoundPlay?.Invoke();
        }
    }
}
