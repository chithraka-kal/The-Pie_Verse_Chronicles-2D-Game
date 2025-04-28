using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform rayCastOrigin;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private LayerMask layerMask;  // Fixed type
    private RaycastHit2D hit2D;

    void Update()
    {
        GroundCheckMethod();  // fixed typo
    }

    private void GroundCheckMethod()
    {
        hit2D = Physics2D.Raycast(rayCastOrigin.position, Vector2.down, 100f, layerMask);
        
        if (hit2D)
        {
            Vector2 temp = playerFeet.position;
            temp.y = hit2D.point.y;
            playerFeet.position = temp;
        }
    }
}
