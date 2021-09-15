using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    [SerializeField] private SunSettings sunSettings;
    [SerializeField] private GameObject sunLight;
    [SerializeField] private PlanetSettings[] settings;

    private GameObject sunObject;

    private int orbitingPlanets;
    
    private Vector3 direction;
    private float speed;

    private List<Planet> planets = new List<Planet>();
    private List<float> planetPos = new List<float>();

    void Start()
    {
        SetDriftValues();
        SpawnSun();
        SpawnPlanets();
    }

    private void Update()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
            transform.Translate(direction * speed * Time.deltaTime);
    }

    private void SetDriftValues()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        speed = Random.Range(2f, 20f);
    }

    void SpawnSun()
    {
        sunObject = ObjectPooler.GetSun();
        sunObject.transform.position = transform.position;
        sunObject.transform.parent = transform;
        sunObject.SetActive(true);
    }

    void SpawnPlanets()
    {

        orbitingPlanets = Random.Range(1, settings.Length);

        Vector3 position = sunObject.transform.position;
        position.x += (Random.Range(5, 15));
        for (int i = 0; i < orbitingPlanets; i++)
        {
            GameObject orbitPoint = ObjectPooler.GetOrbitPoint();
            orbitPoint.transform.position = sunObject.transform.position;
            orbitPoint.transform.parent = sunObject.transform;
            orbitPoint.SetActive(true);
            GameObject planetObj = ObjectPooler.GetPlanet();
            planets.Add(planetObj.GetComponent<Planet>());
            planets[i].SetOrbitPoint(orbitPoint);
            position.x += Random.Range(15f, 25f);
            planetObj.transform.position = position;
            planetObj.SetActive(true);
            planetPos.Add(position.x - sunObject.transform.position.x);
        }
    }
    
    public void ReplacePlanets()
    {
        for (int i = 0; i < planets.Count; i++)
        {
            if (!planets[i].gameObject.activeInHierarchy)
            {
                GameObject planetObj = ObjectPooler.GetPlanet();
                GameObject orbitPoint = ObjectPooler.GetOrbitPoint();
                orbitPoint.transform.position = sunObject.transform.position;
                orbitPoint.transform.parent = sunObject.transform;
                orbitPoint.SetActive(true);
                planets[i] = planetObj.GetComponent<Planet>();
                planets[i].SetOrbitPoint(orbitPoint);
                planetObj.SetActive(true);
                Vector3 position = sunObject.transform.position;
                position.x += planetPos[i];
                planetObj.transform.position = position;
            }
        }

    }


}
