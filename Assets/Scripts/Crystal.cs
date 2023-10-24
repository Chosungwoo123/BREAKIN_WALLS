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
        // 만약 자석이 활성화 되면 플레이어 쪽으로 이동
        if (isMagnet)
        {
            Vector3 dir = GameManager.Instance.curPlayer.transform.position - transform.position;

            transform.position += dir.normalized * 27 * Time.deltaTime;
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