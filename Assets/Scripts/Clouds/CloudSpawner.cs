using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Transform spawnPos;


    private void Start()
    {
        SpawnCloud();
    }

    private void SpawnCloud()
    {
        GameObject newCloud = Instantiate(cloudPrefab, spawnPos.position, quaternion.identity);
        Cloud cloud = newCloud.GetComponent<Cloud>();
        cloud.SpawnPosition = spawnPos.position;
    }

}
