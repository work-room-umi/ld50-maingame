using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class ScoreManager : MonoBehaviour
    {
        public struct Score
        {
            public float TotalTime { get; set; }
            public bool IsStarted { get; set; }
        }

        private static ScoreManager instance;

        private Score totalScore = new Score();
        private ScoreManager()
        {
            Debug.Log("Create ScoreManager GameObject instance.");
            ClearScore();
        }
        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("ScoreManager");
                    instance = go.AddComponent<ScoreManager>();
                }

                return instance;
            }
        }

        public void Update()
        {
            if (totalScore.IsStarted)
            {
                totalScore.TotalTime += Time.deltaTime;
            }
        }

        public void StartScore()
        {
            //Debug.Log("StartScore");
            totalScore.IsStarted = true;
        }

        public void StopScore()
        {
            //Debug.Log("StopScore");
            totalScore.IsStarted = false;
        }

        public void ClearScore()
        {
            //Debug.Log("ClearScore");
            totalScore.TotalTime = 0;
            totalScore.IsStarted = false;
        }
    }
}