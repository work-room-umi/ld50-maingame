using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace umi.ld50
{
    public class MoveScene : MonoBehaviour
    {
        private static Canvas fadeCanvas;
        private static Image fadeImage;
        private static float alpha = 0.0f;

        public string nextScene;
        public float waitFade = 0.5f;
        public float fadeinTime = 0.5f;
        public float fadeoutTime = 0.5f;
        private static bool isFadeIn = false;
        private static bool isFadeOut = false;

        public void OnClickButton()
        {
            StartCoroutine(DelayCoroutine());
        }

        private IEnumerator DelayCoroutine()
        {
            //待つ
            yield return new WaitForSeconds(waitFade);
            
            //n秒かけて切り替わる(ディゾルブ)
            FadeOut();
        }
        static void Init()
        {
            //フェード用のCanvas生成
            GameObject FadeCanvasObject = new GameObject("CanvasFade");
            fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
            FadeCanvasObject.AddComponent<GraphicRaycaster>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            FadeCanvasObject.AddComponent<MoveScene>();

            //最前面になるよう適当なソートオーダー設定
            fadeCanvas.sortingOrder = 100;

            //フェード用のImage生成
            fadeImage = new GameObject("ImageFade").AddComponent<Image>();
            fadeImage.transform.SetParent(fadeCanvas.transform, false);
            fadeImage.rectTransform.anchoredPosition = Vector3.zero;

            //Imageサイズは適当に大きく設定してください
            fadeImage.rectTransform.sizeDelta = new Vector2(9999, 9999);
        }

        private void FadeIn()
        {
            if (fadeImage == null) Init();
            fadeImage.color = Color.black;
            isFadeIn = true;
        }

        private void FadeOut()
        {
            if (fadeImage == null) Init();
            fadeImage.color = Color.clear;
            fadeCanvas.enabled = true;
            isFadeOut = true;
        }

        // void Start()
        // {
        //     FadeIn();
        // }
        void Update()
        {
            if (isFadeIn)
            {
                alpha -= Time.deltaTime / fadeinTime;

                if (alpha <= 0.0f)
                {
                    isFadeIn = false;
                    alpha = 0.0f;
                    fadeCanvas.enabled = false; 
                }

                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            }
            else if(isFadeOut)
            {
                alpha += Time.deltaTime / fadeoutTime;

                if(alpha >= 1.0f)
                {
                    isFadeOut = false;
                    alpha = 1.0f;

                    SceneManager.LoadScene(nextScene);
                }
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            }
        }
    }
}
