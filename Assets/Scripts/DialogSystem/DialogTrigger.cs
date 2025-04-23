using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialogScript;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            dialogScript.gameObject.SetActive(true); // Show the dialog UI
            dialogScript.StartDialog();
        }
    }
}
