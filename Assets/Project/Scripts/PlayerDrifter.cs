using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class PlayerDrifter : MonoBehaviour
    {

        public PlayerShip ship;
        private Animation anim;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animation>();
            if(ship!= null && anim != null)
            {
                ship.BreakPartsAction += OnDamage;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void OnDamage()
        {
            anim.Play();
        }
    }
}
