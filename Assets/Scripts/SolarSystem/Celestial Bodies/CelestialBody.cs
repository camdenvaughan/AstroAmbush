using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class CelestialBody : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    public bool autoUpdate = true;

    protected IShapeGenerator shapeGenerator;

    [HideInInspector] public bool shapeSettingsFoldOut;
    
    [SerializeField, HideInInspector]
    protected MeshFilter[] meshFilters;
    protected TerrainFace[] terrainFaces;

    protected Vector3 rotationAxis;
    protected float rotationSpeed;

    public float planetRadius;

    [SerializeField] protected float health = 100f;



    public virtual void SetupPlanet(int resolution,  CelestialSettings settings)
    {
        this.resolution = resolution;
        this.planetRadius = settings.planetRadius;
        
        rotationAxis.x = Random.Range(0, 1.1f);
        rotationAxis.y = Random.Range(0, 1.1f);
        rotationAxis.z = Random.Range(0, 1.1f);
        rotationSpeed = Random.Range(2, 10) / settings.planetRadius;

        gameObject.tag = "Planet";
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = settings.planetRadius / 10;
        collider.isTrigger = true;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        
    }
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        UpdateColors();
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    protected virtual void Initialize()
    {
        Debug.Log("Initialize not Implemented in Child");
    }


    public void OnSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
            UpdateColors();
        }
    }

    protected virtual void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    protected virtual void UpdateColors()
    {
        Debug.Log("Update Colors not implemented in Child");
    }


}
