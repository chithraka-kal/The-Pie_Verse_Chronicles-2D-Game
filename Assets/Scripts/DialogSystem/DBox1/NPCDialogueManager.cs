using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance;

    public Image npcCharacterIcon;
    public TextMeshProUGUI npcCharacterName;
    public TextMeshProUGUI npcDialogueArea;

    private Queue<DialogueLine> npcLines;

    public Animator npcAnimator;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        npcLines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        npcAnimator.Play("show");  // Play the NPC DBox show animation

        npcLines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            npcLines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        source.Play();  // Play the sound effect for the dialogue
        if (npcLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = npcLines.Dequeue();

        npcCharacterIcon.sprite = currentLine.character.icon;
        npcCharacterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        npcDialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            npcDialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        npcAnimator.Play("hide");  // Play hide animation
    }
}
