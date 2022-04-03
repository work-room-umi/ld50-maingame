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
        private Drifter _shipDrifter;
        [SerializeField] private PlayerShipCollider shipCollider;
        [SerializeField]
        private int growthLevel = 0;

        public float Hp => _parts.Select(p => p.Hp).Sum();
        public float NormalizedHp => _parts.Select(p => p.Hp).Sum() / _parts.Select(p => p.MaxHp).Sum();

        #region Adjust

        [SerializeField] private float growthWidth = 0.1f;
        [SerializeField] private int colliderGrowthPartsCount = 90;
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
            _shipDrifter = GetComponent<Drifter>();
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
            // _parts.Pop(fix);
            // Destroy(fix.gameObject);
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
            
            //Collider Growth
            var newLevel = _parts.Count / colliderGrowthPartsCount;
            growthLevel = newLevel;
            var width = shipCollider.gameObject.GetComponent<MeshRenderer>().bounds.extents.x * 2f;
            var targetWidth = width + growthWidth * newLevel;
            var scale = targetWidth / width;
            shipCollider.gameObject.transform.localScale = new Vector3(scale, scale,scale);
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
            // if (attack._attackableCount <= 0) return;
            // attack._attackableCount--;
            //
            // var targets = new List<Fix> {touchFix};
            // var targetNum = attack.AttackTargetNum-1;
            // var power = attack.AttackPower;
            //
            // //接触したFixに近いFixから攻撃ターゲット数個ピックアップする
            // for (var i = 0; i < targetNum; i++)
            // {
            //     Fix nearestFix = null;
            //     var minDist = float.PositiveInfinity;
            //     //近いfixを検索
            //     foreach (var fix in _parts)
            //     {
            //         if (targets.Contains(fix)) continue;
            //         var d = Vector3.Distance(fix.transform.position, touchFix.transform.position);
            //         if (d < minDist)
            //         {
            //             minDist = d;
            //             nearestFix = fix;
            //         }
            //     }
            //     targets.Add(nearestFix);
            // }
            //
            // foreach (var t in targets)
            // {
            //     if (t == null) continue;
            //     var isCrushed = t.AddDamage(power);
            //     if (isCrushed)
            //     {
            //         FinalizeFix(t);
            //     }
            // }
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
    }
}
