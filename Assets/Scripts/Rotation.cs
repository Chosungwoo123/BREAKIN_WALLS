using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private float z;
    
    private void Update()
    {
        // 계속해서 왼쪽으로 돌림
        z += Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -z);
    }
}