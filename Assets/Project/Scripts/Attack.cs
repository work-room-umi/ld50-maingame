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
        public event Action OnAttacked;
        void Start()
        {
            // 船へ連続的なダメージを与えるためにshipを取得する
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

        public void InformDoneAttacking()
        {
            OnAttacked?.Invoke();
        }
    }
}
