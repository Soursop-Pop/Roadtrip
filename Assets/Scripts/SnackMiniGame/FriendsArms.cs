using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FriendArms : MonoBehaviour
{
    public TMP_Text snackListText;      // UI Text element for listing delivered snacks (e.g., top-right)
    public TMP_Text friendCommentText;  // UI Text element for the friend’s comment

    private List<string> snackNames = new List<string>();

    // Array of possible friend comments
    public string[] friendComments = {
        "Mmm, that's tasty!",
        "Oh, you know I love snacks!",
        "Keep 'em coming!",
        "Whoa, that's a lot!"
    };

    public void AddSnack(string snackName)
    {
        snackNames.Add(snackName);
        UpdateSnackListUI();
        DisplayFriendComment();
    }

    void UpdateSnackListUI()
    {
        snackListText.text = "";
        foreach (string name in snackNames)
        {
            snackListText.text += name + "\n";
        }
    }

    void DisplayFriendComment()
    {
        int index = Random.Range(0, friendComments.Length);
        friendCommentText.text = friendComments[index];
        StartCoroutine(ClearCommentAfterDelay(3f));
    }

    IEnumerator ClearCommentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        friendCommentText.text = "";
    }
}