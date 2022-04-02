#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WorkRoomuUmi.Develop
{
    public static class ColoredProjectView
    {
        const string MenuPath = "Develop/Editor Custom/ColoredProjectView (Kind)";
        static readonly string[] Keywords = {
            "scene",
            "material",
            "editor",
            "resource",
            "prefab",
            "shader",
            "script",
            "model",
            "texture"
        };

        const float alphaIntensity = 0.3f;

        [MenuItem(MenuPath)]
        static void ToggleEnabled()
        {
            Menu.SetChecked(MenuPath, !Menu.GetChecked(MenuPath));
        }

        [InitializeOnLoadMethod]
        static void Init()
        {
            SetEvent();
        }

        static void SetEvent()
        {
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        static void OnGUI(string guid, Rect selectionRect)
        {
            if (!Menu.GetChecked(MenuPath))
            {
                return;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var pathLevel = CountWord(assetPath, "/");

            var originalColor = GUI.color;
            var originalBackground = Texture2D.whiteTexture;

            GUI.color = GetColor(pathLevel, assetPath);
            GUI.skin.box.normal.background = Texture2D.whiteTexture;

            GUI.Box(selectionRect, string.Empty);

            GUI.color = originalColor;
            GUI.skin.box.normal.background = originalBackground;
        }

        static int CountWord(string source, string word)
        {
            return source.Length - source.Replace(word, "").Length;
        }

        static Color GetColor(in int pathLevel, in string assetPath)
        {
            int id, depth;
            (id, depth) = GetColorIdAndDepth(pathLevel, assetPath);

            Color color = (EditorGUIUtility.isProSkin)
                ? GetColorForDarkSkin(id)
                : GetColorForLightSkin(id);
            color.a *= alphaIntensity;
            return color;
        }

        static bool isDirectory(string assetPath)
        {
            return Directory.Exists(assetPath);
        }

        static (int id, int depth) GetColorIdAndDepth(in int pathLevel, in string assetPath)
        {
            if (isDirectory(assetPath))
            {
                int depthBase = 0;
                string[] folderNames = assetPath.Split('/').Reverse().ToArray();
                foreach (string folderName in folderNames)
                {
                    var lowerName = folderName.ToLower();
                    for (int i = 0; i < Keywords.Length; ++i)
                    {
                        if (lowerName.StartsWith(Keywords[i]))
                        {
                            return (i, pathLevel - depthBase);
                        }
                    }
                    ++depthBase;
                }
            }
            return (-1, 0);
        }

        static Color GetColorForDarkSkin(in int id)
        {
            if(id<0){
                return new Color(0, 0, 0, 0);
            }
            float h = ((float)id/(float)Keywords.Length);
            var result = Color.HSVToRGB(h,1f,1f);
            return result;
        }

        static Color GetColorForLightSkin(in int id)
        {
            if(id<0){
                return new Color(0, 0, 0, 0);
            }
            float h = ((float)id/(float)Keywords.Length);
            var result = Color.HSVToRGB(h,1f,1f);
            return result;
        }
    }
}
#endif
