using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float radius = 1;

    public Vector2 regionSize = Vector2.one;

    public int rejectionSamples = 30;

    public float displayRadius = 1;

    private List<Vector2> points;

    public GameObject asteroidPrefab;
    
    [HideInInspector] public GameObject[] asteroids;
    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        
    }

    private void Start()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        if (points != null)
        {
            if (asteroids.Length < points.Count)
            {
                asteroids = new GameObject[points.Count];
                for (int i = 0; i < asteroids.Length; i++)
                {
                    asteroids[i] = Instantiate(asteroidPrefab, transform);
                }
            }
            for (int i = 0; i < points.Count; i++)
            {
                asteroids[i].transform.position =  new Vector3(points[i].x, points[i].y, transform.position.z);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionSize/2, regionSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(new Vector3(point.x, point.y, transform.position.z), displayRadius);
            }
        }
    }
}
