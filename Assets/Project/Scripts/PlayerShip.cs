using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace umi.ld50
{
    public class PlayerShip : MonoBehaviour
    {
        private List<Fix> _parts;

        public float Hp => _parts.Select(p => p.Hp).Sum();

        #region Adjust

        [SerializeField] private bool debug;

        [SerializeField]
        [Range(0,90)]
        private float deployYAngleRange = 45;

        [SerializeField] private float deployMaxRange = 20;
        [SerializeField] private float deployRotationRandomContribution = 0.1f;
        [SerializeField] private Mesh[] debugMeshes;
        #endregion
        
        private void Start()
        {
            //子にあるFixをデフォルトのパーツとして登録/初期化
            var fixes = GetComponentsInChildren<Fix>().ToArray();
            _parts = new List<Fix>();
            _parts.AddRange(fixes);
            _parts.ForEach(InitFix);
        }

        private void InitFix(Fix fix)
        {
            //被ダメージ時の処理登録
            //漂着物取得時登録
            fix.onTouchAttack += OnAttacked;
            fix.onTouchFix += AddFix;
        }

        private void FinalizeFix(Fix fix)
        {
            _parts.Remove(fix);
            Destroy(fix.gameObject);
        }

        private void AddFix(Fix newFix)
        {
            if (_parts.Contains(newFix)) return;
            //子に追加して位置/回転を調整
            newFix.transform.parent = transform;
            newFix.GetComponent<Rigidbody>().isKinematic = true;
            InitFix(newFix);
            DeployFix(newFix);//位置/回転計算
            _parts.Add(newFix);
        }

        private void DeployFix(Fix fix)
        {
            var trans = fix.transform;
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            //船体の外からSphereCast
            var shipCenter = trans.position;
            var castStartPos = CalCastStart() + shipCenter;
            var dir = (shipCenter - castStartPos).normalized;
            var ray = new Ray(castStartPos, dir);
            var radius = fix.GetBoundingRadius();
            var isHit = Physics.SphereCast(ray, radius, out var hit, deployMaxRange);
            if (!isHit) 
                throw new Exception("回収した漂着物の設置位置計算失敗。船のドームのコライダーに当たるはずなので設定ミスてるかも？");
            
            var fixTrans = fix.transform;
            fixTrans.position = hit.point + hit.normal * radius;
            fixTrans.LookAt(hit.point + hit.normal * 5f);
            //少しランダム回転を付ける
            fixTrans.rotation = Quaternion.Lerp(fixTrans.rotation, Random.rotation, deployRotationRandomContribution);
        }
        
        private Vector3 CalCastStart()
        {
            var yRangeScale = deployYAngleRange / 90f;
            return CalCastStart(
                Random.Range(0, Mathf.PI * 2f),//0~360
                Random.Range(0, Mathf.PI * yRangeScale)//0~deployYAngleRange(要調整)
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

        private void OnAttacked(Attack attack, Fix touchFix)
        {
            if (attack._attackableCount <= 0) return;
            attack._attackableCount--;
            
            var targets = new List<Fix> {touchFix};
            var targetNum = attack.AttackTargetNum-1;
            var power = attack.AttackPower;
            
            //接触したFixに近いFixから攻撃ターゲット数個ピックアップする
            for (var i = 0; i < targetNum; i++)
            {
                Fix nearestFix = null;
                var minDist = float.PositiveInfinity;
                //近いfixを検索
                foreach (var fix in _parts)
                {
                    if (targets.Contains(fix)) continue;
                    var d = Vector3.Distance(fix.transform.position, touchFix.transform.position);
                    if (d < minDist)
                    {
                        minDist = d;
                        nearestFix = fix;
                    }
                }
                targets.Add(nearestFix);
            }

            foreach (var t in targets)
            {
                if (t == null) continue;
                var isCrushed = t.AddDamage(power);
                if (isCrushed)
                {
                    FinalizeFix(t);
                }
            }
        }

        #region Debug
        
        [ContextMenu("AddFix")]
        public void AddRandomFix()
        {   
            var go = new GameObject("TestFix");
            var newFix = go.AddComponent<Fix>();
            var filter = go.GetComponent<MeshFilter>();
            var meshRenderer = go.GetComponent<MeshRenderer>();
            if (debugMeshes.Length == 0)
            {
                Debug.LogError("Debug Meshが設定されていないので、Fixの設置テストが出来ません。PlayerShipのInspectorからDebugMeshesを設定して下さい。");
                return;
            }
            var index = Random.Range(0, debugMeshes.Length);
            filter.mesh = debugMeshes[index];
            meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AddFix(newFix);
        }
        private void OnDrawGizmosSelected()
        {
            if(!debug) return;
            //設置Rayの領域をDebug
            var shipCenter = transform.position;
            var yRangeScale = deployYAngleRange / 90f;
            var resolution = 36f;
            for (var i = 0; i < resolution; i++)
            {
                var ration = i / resolution;
                var xzRad = Mathf.PI * 2f * ration;
                var yRad  = Mathf.PI * yRangeScale;
                var castStartPosTop    = CalCastStart(xzRad, yRad) + shipCenter;
                var castStartPosBottom = CalCastStart(xzRad, 0) + shipCenter;
                Gizmos.DrawLine(shipCenter,castStartPosTop);
                Gizmos.DrawLine(shipCenter,castStartPosBottom);
            }
        }
        #endregion
    }
}
