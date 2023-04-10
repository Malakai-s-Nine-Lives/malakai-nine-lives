using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeScene()
    {
        Debug.Log("click");
        int sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        Debug.Log(sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}
