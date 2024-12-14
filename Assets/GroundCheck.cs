using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    
    [SerializeField] private Transform rayCastOrigin;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private Transform layerMask;
    private RaycastHit2D Hit2D;

    

    // Update is called once per frame
    void Update()
    {
        GrroundCheckMethod();
    }

        private void GrroundCheckMethod(){
        Hit2D = Physics2D.Raycast(rayCastOrigin.position, -Vector2.up, 100f, layerMask.gameObject.layer);
        if(Hit2D != false){
            Vector2 temp = playerFeet.position;
            temp.y = Hit2D.point.y;
            playerFeet.position = temp;
        }
    }
}
