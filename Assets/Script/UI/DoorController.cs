using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Malakai")
        {
            // save current health of malakai to the game global if in hard mode
            if (PlayerPrefs.GetString("mode") == "hard")
            {
                PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
                health.SaveHealth();
            }
            
            // load the next scene (cutscene before the next level)
            SceneManager.LoadScene(nextScene);
        }
            
    }
}
