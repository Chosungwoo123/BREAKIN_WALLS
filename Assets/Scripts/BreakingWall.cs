using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingWall : MonoBehaviour
{
    [SerializeField] private GameObject breakingEffectPrefab;
    [SerializeField] private AudioClip breakSound;

    public void Breaking()
    {
        // 이펙트 및 스코어
        GameManager.Instance.CameraShake(0.2f, 4);
        GameManager.Instance.GetScore(130);
        GameManager.Instance.breakingWalls++;

        // 사운드
        SoundManager.Instance.PlaySound(breakSound);
        
        Instantiate(breakingEffectPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}