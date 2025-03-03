using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load after fade out.")]
    public string sceneToLoad = "YourSceneName";

    [Tooltip("Full-screen UI Image used for fading. It should cover the screen and start with 0 alpha.")]
    public Image fadeImage;

    [Tooltip("Duration of the fade out in seconds.")]
    public float fadeDuration = 1f;

    [Tooltip("AudioSource that plays the music, which will be faded out.")]
    public AudioSource musicSource;

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check for the "Vehicle" tag (change this as needed)
        if (!isTransitioning && other.CompareTag("Vehicle"))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        isTransitioning = true;

        // Make sure the fade image is active and starts with 0 alpha
        if (fadeImage)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }

        // Get the starting volume from the music source if it exists
        float timer = 0f;
        float startVolume = musicSource ? musicSource.volume : 0f;

        // Gradually increase the alpha (fade out the screen) and fade out the music
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            if (fadeImage)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }

            if (musicSource)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, alpha);
            }
            yield return null;
        }

        // After the fade-out, load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
