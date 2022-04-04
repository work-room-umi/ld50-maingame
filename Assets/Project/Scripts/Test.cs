using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class Test : MonoBehaviour
    {
        public EnemyManager Manager;

        private void Update()
        {
            if (Manager == null) Manager = FindObjectOfType<EnemyManager>();
            Debug.Log(Manager.NormalizedShipDistance());
        }
    }
}
