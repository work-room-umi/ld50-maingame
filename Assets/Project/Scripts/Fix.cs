using UnityEngine;

namespace umi.ld50
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Fix : MonoBehaviour
    {
        [SerializeField]
        public VFXEmitterComponent _emitter;

        [SerializeField]
        private float _hp = 5;

        [SerializeField] private float _maxHp = 5;
        public float MaxHp => _maxHp;
        public float Hp => _hp;

        // public delegate void OnTouchAttack(Attack attack, Fix touchFix);
        // public delegate void OnTouchFix(Fix fix);
        //
        // public OnTouchAttack onTouchAttack;
        // public OnTouchFix onTouchFix;

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
            _maxHp = _hp;
            _emitter = GetComponentInChildren<VFXEmitterComponent>();
        }

        public bool AddDamage(float damage)
        {
            _hp -= damage;
            var isCrushed = _hp < 0;
            if (isCrushed) _hp = 0;
            return isCrushed;
        }

        // public float GetBoundingRadius()
        // {
        //     var meshRenderer = GetComponent<MeshRenderer>();
        //     var extents = meshRenderer.bounds.extents;
        //     var minExtent = Mathf.Min(Mathf.Min(extents.x, extents.y), extents.z);
        //     var maxExtent = Mathf.Max(Mathf.Max(extents.x, extents.y), extents.z);
        //     return (minExtent + maxExtent) * 0.5f;
        // }
        //
        // public Bounds GetBoundingBox()
        // {
        //     var meshRenderer = GetComponent<MeshRenderer>();
        //     return meshRenderer.bounds;
        // }

        // private void OnCollisionEnter(Collision collision)
        // {
        //     var attack = collision.gameObject.GetComponent<Attack>();
        //     var fix = collision.gameObject.GetComponent<Fix>();
        //     if (attack == null && fix == null) return;
        //
        //     var isAttacked = fix == null;
        //     if (isAttacked && onTouchAttack != null)
        //         onTouchAttack(attack, this);//PlayerShipが処理
        //     
        //     if (!isAttacked && onTouchFix != null)
        //         onTouchFix(fix);//PlayerShipが処理
        // }

        // private void OnDestroy()
        // {
        //     // onTouchAttack = null;
        //     // onTouchFix = null;
        // }

        public void EmitVFX()
        {
            _emitter.Emit();
        }
    }
}
