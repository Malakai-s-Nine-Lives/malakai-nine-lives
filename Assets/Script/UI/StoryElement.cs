using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryElement : MonoBehaviour
{
    public Dialogue dialogue1;
    public Dialogue dialogue2;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue1, dialogue2);
    }
}
