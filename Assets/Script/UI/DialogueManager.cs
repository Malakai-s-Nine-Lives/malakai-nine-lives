using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;

    private Queue<string> sentences;

    private string name1;
    private string name2;

    private Sprite head1;
    private Sprite head2;

    // auto start on first character dialogue given
    private bool isCharacter1 = true;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue1, Dialogue dialogue2)
    {
        // start with dialogue1 name and sprite
        nameText.text = dialogue1.name;
        characterImage.sprite = dialogue1.head;

        // set the name and head of each sprite
        name1 = dialogue1.name;
        name2 = dialogue2.name;
        head1 = dialogue1.head;
        head2 = dialogue2.head;

        sentences.Clear();

        // get collection of sentences from both characters
        string[] list1 = dialogue1.sentences;
        string[] list2 = dialogue2.sentences;

        // queue up sentences in alternating order
        for (int i = 0; i < Mathf.Max(list1.Length, list2.Length); i++)
        {
            if (i < list1.Length)
            {
                sentences.Enqueue(list1[i]);
            }

            if(i < list2.Length)
            {
                sentences.Enqueue(list2[i]);
            }
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // logic to display message from the right character
        nameText.text = isCharacter1 ? name1 : name2;
        characterImage.sprite = isCharacter1 ? head1 : head2;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;

        // switch character that is speaking
        isCharacter1 = !isCharacter1;
    }

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }
}
