using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using static MusicPlayer;

public class SongInfoUI : MonoBehaviour
{
    public static SongInfoUI Instance;

    [Header("UI References")]
    // Panel that will slide in/out. Its anchored position will be animated.
    public RectTransform panelRect;
    // UI text fields for displaying the song information.
    public TMP_Text bandNameText;
    public TMP_Text songTitleText;
    public TMP_Text publisherText;

    // Internal positions for slide animation
    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        Debug.Log("SongInfoUI Awake called");
        if (Instance == null)
        {
            Instance = this;
            if (panelRect != null)
            {
                // Assume the panel's starting position in the Editor is its "shown" position.
                shownPosition = panelRect.anchoredPosition;
                // For testing, hide the panel further off-screen. Increase the multiplier for a more pronounced shift.
                hiddenPosition = new Vector2(-panelRect.rect.width * 2f, shownPosition.y);
                panelRect.anchoredPosition = hiddenPosition;
                Debug.Log("Panel Rect initialized. Shown pos: " + shownPosition + ", Hidden pos (testing): " + hiddenPosition);
            }
            else
            {
                Debug.LogWarning("panelRect is not assigned in the Inspector");
            }
        }
        else
        {
            Debug.LogWarning("Duplicate SongInfoUI instance found. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this method with the SongInfo to update the text and trigger the slide animation.
    /// </summary>
    public void DisplaySongInfo(SongInfo info)
    {
        Debug.Log("DisplaySongInfo called with: " + (info != null ? info.songTitle : "null info"));
        if (info == null) return;

        bandNameText.text = info.bandName;
        songTitleText.text = info.songTitle;
        publisherText.text = info.publisher;

        // Stop any running animation and start a new slide animation
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            Debug.Log("Stopped existing slide animation coroutine.");
        }
        currentCoroutine = StartCoroutine(SlideInThenSlideOut());
        Debug.Log("Started SlideInThenSlideOut coroutine.");
    }

    private IEnumerator SlideInThenSlideOut()
    {
        // For testing, increase the slide duration so you can better see the movement.
        float slideDuration = 3f;  // increased from 1 second to 3 seconds
        float displayDuration = 5f;  // reduced from 30 seconds for testing (adjust as needed)
        float t = 0f;
        Debug.Log("Sliding in...");

        // Slide in: Lerp from hidden to shown position.
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            panelRect.anchoredPosition = Vector2.Lerp(hiddenPosition, shownPosition, t / slideDuration);
            yield return null;
        }
        panelRect.anchoredPosition = shownPosition;
        Debug.Log("Panel slid in to position: " + shownPosition);

        // Wait for the display duration.
        yield return new WaitForSeconds(displayDuration);
        Debug.Log("Display duration ended. Sliding out...");

        // Slide out: Lerp from shown back to hidden position.
        t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            panelRect.anchoredPosition = Vector2.Lerp(shownPosition, hiddenPosition, t / slideDuration);
            yield return null;
        }
        panelRect.anchoredPosition = hiddenPosition;
        Debug.Log("Panel slid out to position: " + hiddenPosition);
    }
}
