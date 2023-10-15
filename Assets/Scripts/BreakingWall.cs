using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    [SerializeField] private GameObject breakingEffectPrefab;

    public void Breaking()
    {
        GameManager.Instance.CameraShake(0.2f, 4);
        
        Instantiate(breakingEffectPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}