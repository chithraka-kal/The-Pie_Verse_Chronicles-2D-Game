using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public Dialogue npcDialogue;

    public void TriggerDialogue()
    {
        NPCDialogueManager.Instance.StartDialogue(npcDialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
}
