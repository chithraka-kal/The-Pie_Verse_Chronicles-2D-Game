using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialogScript; // Reference to your dialog script
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            dialogScript.gameObject.SetActive(true); // In case it's inactive
            dialogScript.StartDialog(); // Call a method to start dialog
            Time.timeScale = 0f; // Pause game
        }
    }
}
