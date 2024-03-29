﻿using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Planet : CelestialBody
{
    public PlanetSettings planetSettings;

    // Color Generator
    private Texture2D texture;
    private const int textureRes = 50;
    

    private void OnValidate()
    {
        GeneratePlanet();
    }

    public override void SetupPlanet(int resolution, CelestialSettings settings)
    {
        this.planetSettings = (PlanetSettings)settings;
        shapeGenerator = new PlanetShapeGenerator();
        base.SetupPlanet(resolution, planetSettings);
    }

    public void SetOrbitPoint(GameObject orbitPoint)
    {
        transform.parent = orbitPoint.transform;
    }
    
    private void Rotate()
    {
        if (!shouldRotateOnSpawn)
        {
            shouldRotateOnSpawn = GameManager.GetState() == GameManager.GameState.Active;
            return;
        }
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
    protected override void Initialize()
    {
        shapeGenerator = new PlanetShapeGenerator();

        // Create Noise Filters and Create Elevation Min Max
        shapeGenerator.UpdateSettings(planetSettings, planetRadius);
        
        // Generate Material
        if (planetSettings.planetMaterial == null)
        {
            planetSettings.planetMaterial = new Material(planetSettings.planetShader);
        }
        
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

    protected override void GenerateMesh()
    {
        base.GenerateMesh();
        UpdateElevation(((PlanetShapeGenerator)shapeGenerator).elevationMinMax);
    }

    protected override void UpdateColors()
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
    
    private void OnTriggerEnter(Collider other)
    {

    }

    public override void DestroyBody(bool spawnDebris)
    {

    }
}
