using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float scanRange;
    public LayerMask scanLayer;

    private void Update()
    {
        ScanUpdate();
    }

    private void ScanUpdate()
    {
        // 범위 안에 있는 크리스탈들의 자성을 활성화
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, scanRange, scanLayer);

        foreach (var target in targets)
        {
            target.GetComponent<Crystal>().isMagnet = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}