using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject continueButton;

    void Start()
    {
        StartCoroutine(Type());
    }

    void Update()
    {
        if (dialogText.text == sentences[index]){
            continueButton.SetActive(true);
        } 
    }

    IEnumerator Type(){
         
        foreach (char letter in sentences[index].ToCharArray()){
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence(){

        continueButton.SetActive(false);

        if (index < sentences.Length - 1){
            index++;
            dialogText.text = "";
            StartCoroutine(Type());
        } else {
            dialogText.text = "";
            continueButton.SetActive(false);
        }
    }
}
