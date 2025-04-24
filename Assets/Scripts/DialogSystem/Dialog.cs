using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public string[] sentences;
    public float typingSpeed = 0.05f;
    public GameObject continueButton;
    public Animator animator;

    private Coroutine typingCoroutine;
    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        dialogText.text = "";
        continueButton.SetActive(false);
    }

    public void StartDialog()
    {
        index = 0;
        dialogText.text = "";
        Time.timeScale = 0f; // Pause game
        gameObject.SetActive(true);
        animator.Play("Dialog_PopIn");

        // Start typing after short delay to let animation finish
        StartCoroutine(DelayedTypeStart(0.3f));
    }

    IEnumerator DelayedTypeStart(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        typingCoroutine = StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        isTyping = true;
        dialogText.text = "";
        continueButton.SetActive(false);

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
        continueButton.SetActive(true);
    }

    public void NextSentence()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogText.text = sentences[index];
            isTyping = false;
            continueButton.SetActive(true);
            return;
        }

        continueButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(Type());
        }
        else
        {
            dialogText.text = "";
            continueButton.SetActive(false);
            Time.timeScale = 1f; // Resume game
            gameObject.SetActive(false);
        }
    }
}
