using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : CelestialBody
{
    public bool autoUpdate = true;

    [HideInInspector] public bool shapeSettingsFoldOut;

    public PlanetSettings planetSettings;

    // Color Generator
    private Texture2D texture;
    private const int textureRes = 50;

    public override void SetupPlanet(int resolution, CelestialSettings settings)
    {
        this.planetSettings = (PlanetSettings)settings;
        shapeGenerator = new PlanetShapeGenerator();
        Debug.Log("Set up planet");
        base.SetupPlanet(resolution, planetSettings);
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    protected override void Initialize()
    {
        // Create Noise Filters and Create Elevation Min Max
        shapeGenerator.UpdateSettings(planetSettings);
        
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

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        UpdateColors();
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
}
