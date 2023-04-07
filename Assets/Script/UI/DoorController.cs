using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool doorOpened = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.Play("open");
        doorOpened = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Malakai" && doorOpened)
        {
            // save current health of malakai to the game global if in hard mode
            if (PlayerPrefs.GetString("mode") == "hard")
            {
                PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
                health.SaveHealth();
            }
            
            // load the next scene (cutscene before the next level)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
            
    }
}
