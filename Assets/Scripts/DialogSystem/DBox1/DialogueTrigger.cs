using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasTriggered = false;

    public bool isFinalDialogue = false; // ← checkbox in inspector
    public GameObject aboutPanel;        // ← assign if isFinalDialogue is true

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, isFinalDialogue, aboutPanel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            TriggerDialogue();
        }
    }
}
