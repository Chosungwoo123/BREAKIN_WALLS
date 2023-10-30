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
        // 왼쪽으로 이동하는 로직
        
        Vector3 movePos = Vector2.left * GameManager.Instance.mapMoveSpeed * Time.deltaTime;

        transform.position += movePos;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 엔드라인에 도달하면 삭제
        if (other.CompareTag("EndLine"))
        {
            Destroy(gameObject);
        }
    }
}