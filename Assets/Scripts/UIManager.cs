using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject repairPrompt;

    void Awake()
    {
        instance = this;
        if (repairPrompt) repairPrompt.SetActive(false);
    }

    public static void ShowRepairPrompt(bool show)
    {
        if (instance.repairPrompt)
            instance.repairPrompt.SetActive(show);
    }
}