using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace umi.ld50
{
    public class RandomImage : MonoBehaviour
    {
        public List<Sprite> Images;
        void Start()
        {
            int random = Random.Range(0, Images.Count); // 0<= random <count
            Image img = this.gameObject.GetComponent<Image>();
            img.sprite = Images[random];
        }
    }
}
