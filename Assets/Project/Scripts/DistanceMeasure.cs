using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace umi.ld50
{
    public class DistanceMeasure : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private PlayerShip _ship;
        [SerializeField] private float enemyNormalizedMaxDistance = 25f;

        public float NormalizedShipDistance()
        {
            var positions = _enemyManager.EnemiesPositions();
            var shipPos = _ship.transform.position;
            var nearestDist = positions.Select(p => Vector3.Distance(p, shipPos)).Min();
            var normalizedDist = Mathf.Clamp01(nearestDist / enemyNormalizedMaxDistance);
            return normalizedDist;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (_enemyManager == null) _enemyManager = FindObjectOfType<EnemyManager>();
            if (_ship == null) _ship = FindObjectOfType<PlayerShip>();
        }
    }
}