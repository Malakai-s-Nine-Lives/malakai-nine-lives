using System;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    // For setting progress and next scene
    public ProgressBar progressBar;
    private int maxPoints = 100;
    public DoorController door;
    private int currentPoints = 0;
    public float progRatio = 0.8f; // ratio of total points need to unlock door

    // Start is called before the first frame update
    void Start()
    {
        // Get enemy game objects
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("enemy");

        if (gameObjects.Length == 0)
        {
            Debug.Log("No game objects are tagged with 'Enemy'");
        }

        int totalPoints = 0;
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<EnemyHealth>())
            {
                // add the point value of the enemy
                totalPoints += gameObject.GetComponent<EnemyHealth>().pointValue;
            }
        }

        // round to nearest int
        maxPoints = Convert.ToInt32(totalPoints * progRatio);

        // Initialize progress bar
        progressBar.SetMaxPoints(maxPoints);
    }

    // Accessed by enemy scripts when they die to award their point amount to the player
    public void TakePoints(int points)
    {
        currentPoints += points;
        progressBar.SetPoints(currentPoints);

        if (currentPoints >= maxPoints)
        {
            // activate the door script
            Debug.Log("Opening door: ");
            door.OpenDoor();
        }
    }
}
