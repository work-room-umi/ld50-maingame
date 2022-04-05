using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class VFXEmitterComponent : MonoBehaviour
    {
        [SerializeField] GameObject prefab;

        [SerializeField] float lifeTime = 3;

        private List<GameObject> instances = new List<GameObject>();
        public void Emit()
        {
            if (prefab == null)
            {
                return;
            }

            var instance = Instantiate(prefab, this.transform.position, Quaternion.identity);
            instances.Add(instance);
            StartCoroutine(DelayAction(lifeTime, instance));
        }

        IEnumerator DelayAction(float lifeTime, GameObject gameObject)
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}
