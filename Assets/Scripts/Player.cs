using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;

    [SerializeField] private Vector2 boundary;

    private void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var curPos = transform.position;

        curPos += new Vector3(h, v, 0) * speed * Time.deltaTime;
        curPos.x = Mathf.Clamp(curPos.x, -boundary.x / 2, boundary.x / 2);
        curPos.y = Mathf.Clamp(curPos.y, -boundary.y / 2, boundary.y / 2);

        transform.position = curPos;
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(Vector3.zero, boundary);
    }
}