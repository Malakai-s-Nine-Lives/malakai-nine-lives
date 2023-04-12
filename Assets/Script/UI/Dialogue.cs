using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Not a part of a functional requirement but this module improves gameplay
 */
// Used by DialogueManager to display character dialogue
[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public string[] sentences;
}
