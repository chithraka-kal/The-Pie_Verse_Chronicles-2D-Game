using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue2Trigger : MonoBehaviour
{
    public Dialogue2 dialogue;

    public void TriggerDialogue()
    {
        FindAnyObjectByType<Dialogue2Manager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
}
