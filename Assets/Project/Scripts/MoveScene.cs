using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace umi.ld50
{
    public class MoveScene : MonoBehaviour
    {
        public string sceneName;
        public float waitFade = 0.5f;
        public Sprite fadeImage;
        public bool isFadeIn = false;
        public bool isFadeOut = false;

        public void OnClickButton()
        {
            StartCoroutine(DelayCoroutine());
        }

        private IEnumerator DelayCoroutine()
        {
            //待つ
            yield return new WaitForSeconds(waitFade);
            
            //n秒かけて切り替わる(ディゾルブ)
            Fade();
        }

        private void FadeIn()
        {
            isFadeIn = true;
        }

        private void FadeOut()
        {
            isFadeOut = true;
        }
        private void Fade()
        {
            if (isFadeIn)
            {

            }
            else if(isfadeout)
            {
                
            }
            SceneManager.LoadScene(sceneName);

        }
    }

}
