using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SolarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject solarSystem;

    [SerializeField] Transform shipPos;
    [SerializeField] private Vector3[] startingPositions = new Vector3[4];

    private void Start()
    {
        for (int i = 0; i < startingPositions.Length; i++)
        {
            Instantiate(solarSystem, transform).transform.position = startingPositions[i];
            
        }
    }
}
