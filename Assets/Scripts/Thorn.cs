using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    private float moveSpeed;

    private bool falling = false;

    private void Update()
    {
        if (!falling)
        {
            return;
        }

        FallingUpdate();
    }
    
    private void FallingUpdate()
    {
        Vector3 nextPos = Vector3.down * moveSpeed * Time.deltaTime;

        transform.position += nextPos;
    }

    public void Falling(float _moveSpeed)
    {
        falling = true;

        moveSpeed = _moveSpeed;
    }

    private void OnBecameInvisible()
    {
        if (falling)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().OnDamage();
            
            Destroy(transform.parent.gameObject);
        }
    }
}