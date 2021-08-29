using System;
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



    protected override void Initialize()
    {
        shapeGenerator.UpdateSettings(sunSettings, planetRadius);

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
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Sun":
                Vector3 shipPos = GameManager.GetShipPos();
                float dist = Vector3.Distance(shipPos, transform.position);

                if (dist > 150f)
                    return;
                // Create some type of explosion with noise
                Invoke("SuperNova", 1.5f);
                break;
            case "Ship":
                // Create Some explosion
                GameManager.EndGame();
                break;
            case "Planet":
                break;
        }
    }

    private void SuperNova()
    {
        Vector3 shipPos = GameManager.GetShipPos();
        float dist = Vector3.Distance(shipPos, transform.position);

        if (dist < 25f)
        {
            GameManager.EndGame();
            DestroyBody(false);
        }
        transform.parent.position = new Vector3(GameManager.GetShipPos().x + 400f, 0f, 0f);
    }

    public override void DestroyBody(bool spawnDebris)
    {
        CancelInvoke();
        ObjectPooler.DestroySun(gameObject);
    }
}
