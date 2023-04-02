using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;

    private Queue<string> sentences;

    public GameObject button;

    public void StartDialogue(Dialogue dialogue)
    {
        // clear out queue of sentences
        sentences = new Queue<string>();

        // get collection of sentences from both characters
        string[] list = dialogue.sentences;

        // queue up sentences
        for (int i = 0; i < list.Length; i++)
        {
            sentences.Enqueue(list[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // if there's no more sentences to display, quit
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        // get rid of continue button
        button.SetActive(false);
        Debug.Log("End of Conversation");
    }
}
