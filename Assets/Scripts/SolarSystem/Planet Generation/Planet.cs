using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    public bool autoUpdate = true;
    
    [HideInInspector] public bool shapeSettingsFoldOut;
    [HideInInspector] public bool colorSettingsFoldOut;

    public PlanetSettings planetSettings;

    private ShapeGenerator shapeGenerator = new ShapeGenerator();
    private ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    
    private void OnValidate()
    {
        GeneratePlanet();
    }
    
    public void SetupPlanet(int resolution, PlanetSettings planetSettings)
    {
        this.resolution = resolution;
        this.planetSettings = planetSettings;
    }
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }
    void Initialize()
    {

        shapeGenerator.UpdateSettings(planetSettings);
        colorGenerator.UpdateSettings(planetSettings);

        
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


    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();    
        }
        
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();    
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }
    
}
