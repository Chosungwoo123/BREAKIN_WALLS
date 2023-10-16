using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapCreater : MonoBehaviour
{
    [SerializeField] private GameObject[] mapPrefabs;

    private void Start()
    {
        CreateMap();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Map"))
        {
            CreateMap();
        }
    }
    
    private void CreateMap()
    {
        Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], transform.position, Quaternion.identity);
    }
}