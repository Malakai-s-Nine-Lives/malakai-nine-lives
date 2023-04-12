using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This module satisfies:
 *   - Functional requirement 2.1
 *   - Functional requirement 10.1
 */
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
