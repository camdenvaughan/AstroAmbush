using UnityEngine;


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

    [SerializeField] protected Vector3 rotationAxis;
    [SerializeField] protected float rotationSpeed;

    public float planetRadius;
    public bool shouldRotateOnSpawn = false;

    [SerializeField] protected float health = 100f;



    public virtual void SetupPlanet(int resolution,  CelestialSettings settings)
    {
        this.resolution = resolution;

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = planetRadius / 10;
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
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
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
    }

    public virtual void DestroyBody(bool spawnDebris)
    {
    }

}
