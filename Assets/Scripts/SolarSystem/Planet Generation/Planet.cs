using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    public bool autoUpdate = true;

    [HideInInspector] public bool shapeSettingsFoldOut;
    [HideInInspector] public bool colorSettingsFoldOut;

    public PlanetSettings planetSettings;

    private ShapeGenerator shapeGenerator = new ShapeGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private Vector3 rotationAxis;
    private float rotationSpeed;

    // Color Generator
    private Texture2D texture;
    private const int textureRes = 50;
    
    private void OnValidate()
    {
        //GeneratePlanet();
    }
    
    public void SetupPlanet(int resolution, PlanetSettings planetSettings)
    {
        this.resolution = resolution;
        this.planetSettings = planetSettings;
        rotationAxis.x = Random.Range(0, 1.1f);
        rotationAxis.y = Random.Range(0, 1.1f);
        rotationAxis.z = Random.Range(0, 1.1f);
        rotationSpeed = Random.Range(0, 5) / planetSettings.planetRadius;
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

    void Initialize()
    {
        // Create Noise Filters and Create Elevation Min Max
        shapeGenerator.UpdateSettings(planetSettings);
        
        // Generate Texture for Colors
        if (texture == null)
        {
            texture = new Texture2D(textureRes, 1);
        }

        
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planetSettings.planetMaterial;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
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

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            UpdateColors();    
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void UpdateColors()
    {
        Color[] colors = new Color[textureRes];
        for (int i = 0; i < textureRes; i++)
        {
            colors[i] = planetSettings.gradient.Evaluate(i / (textureRes - 1f));
        }
        texture.SetPixels(colors);
        texture.Apply();
        
        planetSettings.planetMaterial.SetTexture("_texture", texture);
    }

    void UpdateElevation(MinMax elevationMinMax)
    {
        planetSettings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    }


    
}
