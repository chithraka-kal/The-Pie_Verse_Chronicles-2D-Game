using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private AudioSource source;

    private bool shouldShowAboutPanel = false;
    private GameObject aboutPanelToShow;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, bool isFinal = false, GameObject aboutPanel = null)
    {
        isDialogueActive = true;

        animator.Play("show");

        lines.Clear();

        shouldShowAboutPanel = isFinal;
        aboutPanelToShow = aboutPanel;

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        source.Play();

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        if (currentLine.character != null)
        {
            if (currentLine.character.icon != null)
            {
                characterIcon.gameObject.SetActive(true);
                characterIcon.sprite = currentLine.character.icon;
            }
            else
            {
                characterIcon.gameObject.SetActive(false);
            }

            characterName.text = currentLine.character.name;
        }
        else
        {
            characterIcon.gameObject.SetActive(false);
            characterName.text = "";
        }

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");

        if (shouldShowAboutPanel && aboutPanelToShow != null)
        {
            aboutPanelToShow.SetActive(true);
            Debug.Log("Final dialogue ended. About Panel shown.");
        }
    }
}
