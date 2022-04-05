using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    [RequireComponent(typeof(MeshCollider))]
    public class PlayerShipCollider : MonoBehaviour
    {
        public delegate void OnTouchAttack(Attack attack);
        public delegate void OnTouchFix(Fix fix);
        
        public OnTouchAttack onTouchAttack;
        public OnTouchFix onTouchFix;
        private void Start()
        {
            var col = GetComponent<MeshCollider>();
            col.convex = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var go = collision.gameObject;
            var attack = go.GetComponent<Attack>();
            var fix    = go.GetComponent<Fix>();
            if (attack == null && fix == null) return;
            var isAttacked = fix == null;
            
            if (isAttacked && onTouchAttack != null)
                onTouchAttack(attack);//PlayerShipが処理
            
            if (!isAttacked && onTouchFix != null)
                onTouchFix(fix);//PlayerShipが処理
        }
    }
}
