using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace umi.ld50{
    public class DeleyTimeDestroyer : MonoBehaviour
    {
        public float time;
        // Start is called before the first frame update
        void Start()
        {
            AsyncDestroy();
        }

        async void AsyncDestroy(){
            if(this == null) return;
            if(gameObject == null) return;
            await Task.Delay((int)(time*1000f));
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}