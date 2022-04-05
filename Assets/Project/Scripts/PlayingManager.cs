using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace umi.ld50
{
    public class PlayingManager : MonoBehaviour
    {
        private Score score = Score.Instance;
        [SerializeField]
        private PlayerShip playerShip;
        [SerializeField]
        private MoveScene moveScene;
        private bool isGameOver = false;
        // Start is called before the first frame update
        void Start()
        {
            score.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            score.AddScore(Time.deltaTime);
            if(!playerShip.IsInitialized) return;
            if(playerShip.Hp == 0 && !isGameOver)
            {
                Debug.Log(playerShip.IsInitialized);
                Debug.Log(playerShip.Hp);
                moveScene.OnClickButton();
                isGameOver = true;
            }
        }
    }
}
