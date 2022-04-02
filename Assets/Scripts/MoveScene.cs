using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace umi.ld50
{
    public class MoveScene : MonoBehaviour
    {
        public SceneAsset sceneName;
        public void OnClickButton()
        {

            SceneManager.LoadScene(sceneName.name);
        }
    }
}