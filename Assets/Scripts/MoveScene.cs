using UnityEngine;
using UnityEngine.SceneManagement;

namespace umi.ld50
{
    public class MoveScene : MonoBehaviour
    {
        public string sceneName;
        public void OnClickButton()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}