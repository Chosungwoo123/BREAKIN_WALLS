using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        Vector3 movePos = Vector2.left * GameManager.Instance.mapMoveSpeed * Time.deltaTime;

        transform.position += movePos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndLine"))
        {
            Destroy(gameObject);
        }
    }
}