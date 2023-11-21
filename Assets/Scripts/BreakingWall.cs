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
        
        GameManager.Instance.GetScore(130);

        GameManager.Instance.breakingWalls++;
        
        Instantiate(breakingEffectPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}