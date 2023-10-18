using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingThorn : MonoBehaviour
{
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float scanRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Thorn thornObj;

    private bool isCollision;

    private void FixedUpdate()
    {
        ScanUpdate();
    }

    private void ScanUpdate()
    {
        if (isCollision)
        {
            return;
        }
        
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, scanRange, whatIsPlayer);

        if (ray.transform != null)
        {
            Debug.Log("충돌됨!!"); 
            thornObj.Falling(fallingSpeed);
            isCollision = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawRay(transform.position, Vector3.down * scanRange);
    }
}