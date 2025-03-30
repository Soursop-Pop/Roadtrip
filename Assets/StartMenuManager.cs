using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuManager : MonoBehaviour
{
    public TMP_InputField sceneInputField;

    public void StartGame()
    {
        // Replace with your actual first game scene
        SceneManager.LoadScene("Intro");
    }

    public void LoadSceneByInput()
    {
        string sceneName = sceneInputField.text;

        if (!string.IsNullOrEmpty(sceneName) && Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' not found in build settings.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // So it works in editor
#endif
    }
}