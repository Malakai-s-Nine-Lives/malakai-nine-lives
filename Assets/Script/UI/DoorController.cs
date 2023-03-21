using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Malakai")
        {
            // load the next scene (cutscene before the next level)
            SceneManager.LoadScene(nextScene);
        }
            
    }
}
