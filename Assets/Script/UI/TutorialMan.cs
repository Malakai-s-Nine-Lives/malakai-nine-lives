using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMan : MonoBehaviour
{
    public StoryElement story;

    // Start is called before the first frame update
    void Start()
    {
        story.TriggerDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
