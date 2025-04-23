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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

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
        }
    }
}
