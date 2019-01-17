using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText; 
    public Text newsText;

    void Start() {
        ClearDialogue();
        ClearNews();
    }

    public void SetDialogue(string dialogue) {
        dialogueText.text = dialogue;
    }

    public void ClearDialogue() {
        dialogueText.text = "";
    }

    public void ClearNews() {
        newsText.text="";
    }

    public void SetNews(string news) {
        newsText.text=news;
        Invoke("ClearNews", 2f);
    }

    public void DelayClearLog() {
        Invoke("ClearDialogue", 2f);
    }
}
