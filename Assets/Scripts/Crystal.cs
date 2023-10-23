using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private GameObject effect;

    public bool isMagnet;

    private void Update()
    {
        if (isMagnet)
        {
            Vector3 dir = GameManager.Instance.curPlayer.transform.position - transform.position;

            transform.position += dir.normalized * 35 * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GetScore(40);

            Instantiate(effect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}