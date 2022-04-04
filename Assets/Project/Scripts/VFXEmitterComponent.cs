using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class VFXEmitterComponent : MonoBehaviour
    {
        [SerializeField] GameObject prefab;

        [SerializeField] float lifeTime;

        private GameObject instance;
        public void Emit()
        {
            if (prefab == null)
            {
                return;
            }
            instance = Instantiate(prefab, this.transform.position, Quaternion.identity); 
        }
        void PlayParticle()
        {
            StartCoroutine(DelayAction(lifeTime));
        }
        IEnumerator DelayAction(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(this.gameObject);
        }
    }
}
