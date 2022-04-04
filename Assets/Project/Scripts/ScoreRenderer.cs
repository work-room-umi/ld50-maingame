using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace umi.ld50
{
    public class ScoreRenderer : MonoBehaviour
    {
        [SerializeField]
        private Text hour;
        [SerializeField]
        private Text minute;
        [SerializeField]
        private Text second;
        private Score score = Score.Instance;
        private float lastScore;

        // Start is called before the first frame update
        void Start()
        {
            DrawScore();
        }

        // Update is called once per frame
        void Update()
        {
            if(score.TotalScore != lastScore)
            {
                DrawScore();
                lastScore = score.TotalScore;
            }
        }
        void DrawScore()
        {
            TimeSpan ts = TimeSpan.FromSeconds(score.TotalScore);
            hour.text = ((int)ts.TotalHours).ToString("D2");
            minute.text = ts.Minutes.ToString("D2");
            second.text = ts.Seconds.ToString("D2");
        }
    }
}
