using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public struct Score
    {
        public float totalTime;
        public float totalDistance;
        public double totalScore()
        {
            return totalTime * totalDistance;
        }
    }

    private static ScoreManager instance;

    private Score totalScore = new Score();
    private ScoreManager()
    {
        Debug.Log("Create ScoreManager GameObject instance.");
        Reset();
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

    public void Add(float addDistance)
    {
        //Debug.Log("Add");
        totalScore.totalTime += Time.deltaTime;
        totalScore.totalDistance += addDistance;
    }

    public Score Get()
    {
        //Debug.Log("Get");
        return totalScore;
    }

    public void Reset()
    {
        //Debug.Log("Reset");
        totalScore.totalTime = 0;
        totalScore.totalDistance = 0;
    }
}
