using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameEditorDebugHelpers
{
    public class SceneSwitcherEditor : MonoBehaviour
    {

        [MenuItem("SceneSwitcher/Preloader")]
        static void LoadPreloader()
        {
            LoadSceneByName("Preloader");
        }

        [MenuItem("SceneSwitcher/Main menu")]
        static void LoadMainMenu()
        {
            LoadSceneByName("MainMenu");
        }

        [MenuItem("SceneSwitcher/Bonfire scene")]
        static void LoadBonfireScene()
        {
            LoadSceneByName("BonfireScene");
        }

        [MenuItem("SceneSwitcher/GlobalMap")]
        static void LoadGlobalMapScene()
        {
            LoadSceneByName("GlobalMap");
        }

        [MenuItem("SceneSwitcher/Battle Scene")]
        static void LoadBattleScene()
        {
            LoadSceneByName("BattleScene");
        }

        static void LoadSceneByName(string sceneName)
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}.unity");
        }
    }

}