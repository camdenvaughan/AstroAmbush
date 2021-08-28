using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : CelestialBody
{
    private SunSettings sunSettings;
    public override void SetupPlanet(int resolution, CelestialSettings settings)
    {
        base.SetupPlanet(resolution, settings);
        sunSettings = (SunSettings)settings;
        shapeGenerator = new SunShapeGenerator();
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    protected override void Initialize()
    {
        shapeGenerator.UpdateSettings(sunSettings);

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
            
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(sunSettings.planetShader);
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    protected override void UpdateColors()
    {
    }
    
}
