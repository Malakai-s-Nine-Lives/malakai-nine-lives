using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Not a part of a functional requirement but this module improves gameplay
 */
public class StoryElement : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
