using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class Score
    {
        private static Score s_instance;
        public static Score Instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new Score();
                }
                return s_instance;
            }
        }

        private Score()
        {
        }

        public float TotalScore
        {
            get;
            private set;
        } = 0.0f;

        public void AddScore(float score)
        {
            TotalScore += score;
        }
        public void Clear()
        {
            TotalScore = 0;
        }
    }
}
