using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    // For setting progress and next scene
    public ProgressBar progressBar;
    public int maxPoints = 100;
    public DoorController door;
    private int currentPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
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
