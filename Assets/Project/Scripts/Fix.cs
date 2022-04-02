using UnityEngine;

namespace umi.ld50
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody))]
    public class Fix : MonoBehaviour
    {
        [SerializeField]
        private float _hp = 5;
        public float Hp => _hp;

        public delegate void OnTouchAttack(Attack attack, Fix touchFix);
        public delegate void OnTouchFix(Fix fix);

        public OnTouchAttack onTouchAttack;
        public OnTouchFix onTouchFix;

        public bool AddDamage(float damage)
        {
            _hp -= damage;
            var isCrushed = _hp < 0;
            if (isCrushed) _hp = 0;
            return isCrushed;
        }

        public float GetBoundingRadius()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            var extents = meshRenderer.bounds.extents;
            var minExtent = Mathf.Min(Mathf.Min(extents.x, extents.y), extents.z);
            return minExtent;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var attack = collision.gameObject.GetComponent<Attack>();
            var fix = collision.gameObject.GetComponent<Fix>();
            if (attack == null && fix == null) return;

            var isAttacked = fix == null;
            if (isAttacked)
                onTouchAttack(attack, this);//PlayerShipが処理
            else 
                onTouchFix(fix);//PlayerShipが処理
        }

        private void OnDestroy()
        {
            onTouchAttack = null;
            onTouchFix = null;
        }
    }
}
