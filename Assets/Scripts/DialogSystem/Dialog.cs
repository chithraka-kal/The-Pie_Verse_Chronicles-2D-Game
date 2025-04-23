using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

void Start()
{
    if (dialogText == null || continueButton == null)
    {
        Debug.LogError("UI references not assigned!");
        enabled = false;
        return;
    }

    if (sentences.Length > 0)
    {
        typingCoroutine = StartCoroutine(Type());
    }
    else
    {
        Debug.LogWarning("No sentences assigned!");
    }
}


    void Update()
    {
        if (!isTyping && dialogText.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        isTyping = true;
        dialogText.text = "";
        continueButton.SetActive(false);

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
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
    }
    else
    {
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
            Time.timeScale = 1f; // Resume game when dialog ends
            gameObject.SetActive(false); // Optional: Hide dialog box
        }
    }
}

public void StartDialog()
{
    index = 0;
    dialogText.text = "";
    if (typingCoroutine != null)
        StopCoroutine(typingCoroutine);

    typingCoroutine = StartCoroutine(Type());
}


}
