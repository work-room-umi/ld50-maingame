using System;
using UnityEngine;

namespace umi.ld50
{
    //仮（良しなに書き変えて）
    public class Attack:MonoBehaviour
    {
        //Fixにぶつかれば攻撃処理が走る。
        //ぶつかりに行くことが攻撃になる。
        [SerializeField]
        private float _attackPower = 10;//攻撃力

        public float AttackPower => _attackPower;
       
        public int _attackableCount = 1;//攻撃可能回数
        //氷山とかは
        //クールタイム付けて
        public float _attackInterval = 1f; // default 1sec
        //繰り返しダメージ与える
        public bool _repeatAttack;

        PlayerShip _ship;
        float _attackTrigger=0;

        private Collider collider;
        public event Action OnAttacked;

        [SerializeField] private VFXEmitterComponent vfxEmitter;
        void Start()
        {
            if (vfxEmitter == null)
                vfxEmitter = GetComponentInChildren<VFXEmitterComponent>();
            collider = GetComponent<Collider>();
        }

        void Update()
        {
            if (_ship==null){
                _ship =  (PlayerShip)FindObjectOfType(typeof(PlayerShip));
            }
            float t = Time.time;
            if (_repeatAttack && t > _attackTrigger)
            {
                _ship.AddDamage(this);
                _attackTrigger = t + _attackInterval;
            }
        }

        public void SetAttackPower(float p){
            _attackPower = p;
        }
        public void InformDoneAttacking()
        {
            OnAttacked?.Invoke();
        }

        private void EmitVFX(Vector3 position)
        {
            if(vfxEmitter == null) return;
            vfxEmitter.gameObject.transform.position = position;
            vfxEmitter.Emit();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var isColliedToShip = collision.gameObject.layer == LayerMask.NameToLayer("ShipCollider");
            if(!isColliedToShip) return;
            
            var sum = Vector3.zero;
            foreach (var point in collision.contacts)
            {
                sum += point.point;
            }
            var averagePos = sum / collision.contacts.Length;
            EmitVFX(averagePos);
        }
    }
}
