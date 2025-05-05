using UnityEngine;

public class AboutPanelActivator : MonoBehaviour
{
    public GameObject aboutPanel;

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnded += EnableAboutPanel;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnded -= EnableAboutPanel;
    }

    private void EnableAboutPanel()
    {
        if (aboutPanel != null)
        {
            aboutPanel.SetActive(true);
            Debug.Log("About Panel Activated After Dialogue");
        }

        // Optional: disable this object so it doesn't run again
        gameObject.SetActive(false);
    }
}
