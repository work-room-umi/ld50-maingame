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
        
        [SerializeField]
        private int _attackTargetnum = 1;//同時に攻撃するFixの数

        public int AttackTargetNum => _attackTargetnum;

        public int _attackableCount = 1;//攻撃可能回数
        //氷山とかは
        //クールタイム付けて
        //繰り返しダメージ与える?
    }
}
