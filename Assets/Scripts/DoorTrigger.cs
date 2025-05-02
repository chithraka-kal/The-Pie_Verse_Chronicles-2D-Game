using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Start is called before the first frame update
    public class DoorTrigger : MonoBehaviour
{
    public Animator doorAnimator; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("isOpen", true);
            Debug.Log("Door Opened"); // Optional
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("isOpen", false); // Optional
        }
    }
}

