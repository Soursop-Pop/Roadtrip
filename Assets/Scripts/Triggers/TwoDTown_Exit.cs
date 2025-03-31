using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TwoDTown_Exit : MonoBehaviour
{
    public string sceneToLoad = "NextScene";
    public float fadeDuration = 1f;
    public Image fadePanel;

    private bool isFading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isFading)
        {
            Debug.Log("[TwoDTown_Exit] Already fading, ignoring new trigger.");
            return;
        }

        if (other.CompareTag("Vehicle"))
        {
            Debug.Log("[TwoDTown_Exit] Vehicle entered exit zone. Starting fade and scene transition.");
            StartCoroutine(FadeAndLoadScene());
        }
        else
        {
            Debug.Log("[TwoDTown_Exit] Non-vehicle entered exit zone: " + other.name);
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        Debug.Log("[TwoDTown_Exit] Starting fade to black...");

        // Ensure the panel is visible
        fadePanel.gameObject.SetActive(true);

        float t = 0f;
        Color color = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            color.a = alpha;
            fadePanel.color = color;

            Debug.Log($"[TwoDTown_Exit] Fading... alpha: {alpha:F2}");

            yield return null;
        }

        Debug.Log("[TwoDTown_Exit] Fade complete. Loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}