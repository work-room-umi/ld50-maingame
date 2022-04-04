using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace umi.ld50
{
    public class PlayerShip : MonoBehaviour
    {
        private Stack<Fix> _parts;
        [SerializeField] private PlayerShipCollider shipCollider;

        public event Action GetPartsAction;
        public event Action AttackedAction;
        public event Action BreakPartsAction;

        public float Hp => _parts.Select(p => p.Hp).Sum();
        public float NormalizedHp => _parts.Select(p => p.Hp).Sum() / _parts.Select(p => p.MaxHp).Sum();

        #region Adjust
        [SerializeField, Range(0, 1)]
        private float shipCenterYOffset = 0.3f;
        [SerializeField,Range(-90,90)]
        private float rayYnMin = -8;
        [SerializeField, Range(-90,90)]
        private float rayYnMax = 20;

        [SerializeField] private float deployMaxRange = 20;
        [SerializeField] private float deployRotationRandomContribution = 0f;
        [SerializeField] private bool debug;
        #endregion
        
        private void Start()
        {
            //子にあるFixをデフォルトのパーツとして登録/初期化
            var fixes = GetComponentsInChildren<Fix>().ToArray();
            _parts = new Stack<Fix>(fixes);
            if (shipCollider == null)
                shipCollider = FindObjectOfType<PlayerShipCollider>();
            InitShipCollider(shipCollider);
            Physics.queriesHitBackfaces = true;
        }

        private void InitShipCollider(PlayerShipCollider playerShipCollider)
        {
            playerShipCollider.onTouchAttack += OnAttacked;//被ダメージ時の処理登録
            playerShipCollider.onTouchFix += AddFix;//漂着物取得時登録
        }
        
        private void FinalizeFix(Fix fix)
        {
            Destroy(fix.gameObject);
        }

        private void AddFix(Fix newFix)
        {
            if (_parts.Contains(newFix)) return;
            //子に追加して位置/回転を調整
            var fixTrans = newFix.transform;
            var oldParent = fixTrans.parent;//Drifterを削除
            fixTrans.parent = transform;
            Destroy(oldParent.gameObject);
            newFix.GetComponent<Rigidbody>().isKinematic = true;
            DeployFix(newFix);//位置/回転計算
            _parts.Push(newFix);
            newFix.GetComponent<Collider>().enabled = false;

            OnGetPartsAction();
            
            //Collider Growth
            var maxCount = 1000f;
            var maxScaleX = 6f;
            var maxScaleZ = 3f;
            var normalizedScale = Mathf.Clamp01((_parts.Count-20) / maxCount);
            normalizedScale = Mathf.Pow(normalizedScale, 1f);
            var scaleX = 1f + normalizedScale*(maxScaleX-1f);
            var scaleZ = 1f + normalizedScale*(maxScaleZ-1f);
            shipCollider.gameObject.transform.localScale = new Vector3(scaleX, 1,scaleZ);
        }

        private void DeployFix(Fix fix)
        {
            var shipCenter = transform.position + transform.up * shipCenterYOffset;
            var dst = CalCastStart() + shipCenter;
            var ray = new Ray(dst, (shipCenter - dst).normalized);
            var layerMask = LayerMask.GetMask("ShipCollider");
            var isHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity, layerMask);
            if (!isHit)
            {
                Debug.Log("レイは当たりませんでした");
                return;
            }
            var fixTrans = fix.transform;
            fixTrans.position = hit.point;
            fixTrans.LookAt(hit.point + hit.normal * 5f);
            //少しランダム回転を付ける
            fixTrans.rotation = Quaternion.Lerp(fixTrans.rotation, Random.rotation, deployRotationRandomContribution);
        }
        
        private Vector3 CalCastStart()
        {
            var yMinRad = rayYnMin / 180f * Mathf.PI;
            var yMaxRad = rayYnMax / 180f * Mathf.PI;
            return CalCastStart(
                Random.Range(0, Mathf.PI * 2f),//0~360
                Random.Range(yMinRad, yMaxRad)//yの範囲はinspectorで調整
                );
        }

        private Vector3 CalCastStart(float xzRad, float yRad)
        {
            var z = Mathf.Sin(xzRad);
            var x = Mathf.Cos(xzRad);
            var y = Mathf.Sin(yRad);
            //外側20のとこから船の中心にcastして外側に衝突させる
            return new Vector3(x, y, z).normalized * deployMaxRange;
        }

        private void OnAttacked(Attack attack)
        {
            OnAttackedAction();
            OnBreakPartsAction();
            
            if (attack._attackableCount <= 0) return;
            if (_parts.Count == 0) return;
            attack._attackableCount--;
            
            bool emitBreakEvent = false;
            var damage = attack.AttackPower;
            while (0 < damage)
            {
                if (_parts.Count == 0) break;
                var latest = _parts.Pop();
                var hp = latest.Hp;
                var isCrushed = latest.AddDamage(damage);
                
                if (isCrushed)
                {
                    damage -= hp;
                    emitBreakEvent = true;
                    FinalizeFix(latest);
                }
                else
                {
                    _parts.Push(latest);
                    break;
                }
            }
            if(emitBreakEvent) OnBreakPartsAction();
            
        }

        public void AddDamage(Attack attack)
        {
            if (attack._attackableCount <= 0) return;
            if (_parts.Count == 0) return;
            attack._attackableCount--;
            
            bool emitBreakEvent = false;
            var damage = attack.AttackPower;
            while (0 < damage)
            {
                if (_parts.Count == 0) break;
                var latest = _parts.Pop();
                var hp = latest.Hp;
                var isCrushed = latest.AddDamage(damage);
                
                if (isCrushed)
                {
                    damage -= hp;
                    emitBreakEvent = true;
                    FinalizeFix(latest);
                }
                else
                {
                    damage = 0;
                    _parts.Push(latest);
                }
            }
            if(emitBreakEvent) OnBreakPartsAction();
        }

        #region Debug
        private void OnDrawGizmos()
        {
            if(!debug) return;
            //設置Rayの領域をDebug
            var shipCenter = transform.position + transform.up * shipCenterYOffset;
            var yMinRad = rayYnMin / 180f * Mathf.PI;
            var yMaxRad = rayYnMax / 180f * Mathf.PI;
            var resolution = 36f;
            for (var i = 0; i < resolution; i++)
            {
                var ration = i / resolution;
                var xzRad = Mathf.PI * 2f * ration;
                var castStartPosTop    = CalCastStart(xzRad, yMaxRad) + shipCenter;
                var castStartPosBottom = CalCastStart(xzRad, yMinRad) + shipCenter;
                Gizmos.DrawLine(shipCenter,castStartPosTop);
                Gizmos.DrawLine(shipCenter,castStartPosBottom);
            }
        }
        #endregion

        private void OnGetPartsAction()
        {
            GetPartsAction?.Invoke();
        }

        private void OnAttackedAction()
        {
            AttackedAction?.Invoke();
        }

        protected virtual void OnBreakPartsAction()
        {
            BreakPartsAction?.Invoke();
        }
    }
}
