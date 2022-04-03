using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class Test : MonoBehaviour
    {
        public PlayerShip Ship;

        private void Start()
        {
            Ship.AttackedAction += () => Debug.Log("attack");
            Ship.BreakPartsAction += () => Debug.Log("break");
            Ship.GetPartsAction += () => Debug.Log("get");
        }
    }
}
