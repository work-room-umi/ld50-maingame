using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace umi.ld50
{
    public class AssetViewScene : MonoBehaviour
    {
        public List<string> sceneNames;
        public void Start()
        {
            foreach(var name in sceneNames)
            {
                SceneManager.LoadScene(name, LoadSceneMode.Additive);
            }
        }
    }
}
