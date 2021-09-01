using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Planet : CelestialBody
{
    public PlanetSettings planetSettings;

    // Color Generator
    private Texture2D texture;
    private const int textureRes = 50;

    [HideInInspector]
    public GameObject orbitPoint;

    private void OnValidate()
    {
        //GeneratePlanet();
    }

    public override void SetupPlanet(int resolution, CelestialSettings settings)
    {
        this.planetSettings = (PlanetSettings)settings;
        shapeGenerator = new PlanetShapeGenerator();
        base.SetupPlanet(resolution, planetSettings);
    }

    public void SetOrbitPoint(GameObject orbitPoint)
    {
        this.orbitPoint = orbitPoint;
        transform.parent = orbitPoint.transform;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!shouldRotateOnSpawn)
        {
            shouldRotateOnSpawn = GameManager.GameIsActive();
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
        switch (other.tag)
        {
            case "Bullet":
                health -= 30f;
                if (health < 0)
                {
                    DestroyBody(true);
                }
                break;
            case "Planet":
                Vector3 shipPos = GameManager.GetShipPos();
                float dist = Vector3.Distance(shipPos, transform.position);

                if (dist > 77f)
                    return;
                
                DestroyBody(true);
                break;
            case "Sun":
                DestroyBody(true);
                break;
            case "Ship":
                DestroyBody(true);
                GameManager.EndGame();
                break;
        }
    }

    public override void DestroyBody(bool spawnDebris)
    {
        if (spawnDebris)
        {
            GameObject obj = ObjectPooler.GetExplosionObj();
            obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            obj.SetActive(true);
        }
        ObjectPooler.DestroyPlanet(gameObject, orbitPoint);
    }
}
