using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class Dialogue3Character
{
    public string name;
    public Sprite icon;
}
 
[System.Serializable]
public class DialogueLine
{
    public Dialogue3Character character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class Dialogue3Trigger : MonoBehaviour
{
    public Dialogue dialogue;
 
    public void TriggerDialogue()
    {
        Dialogue3Manager.Instance.StartDialogue(dialogue);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            TriggerDialogue();
        }
    }
}