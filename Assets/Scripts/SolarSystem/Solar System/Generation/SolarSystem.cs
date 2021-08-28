using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public class SolarSystem : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    [SerializeField] private SunSettings sunSettings;
    [SerializeField] private GameObject sunLight;
    [SerializeField] private PlanetSettings[] settings;

    private int orbitingPlanets;
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Sun");
        sunObject.transform.parent = transform;
        CelestialBody sun = sunObject.AddComponent<Sun>();
        sun.SetupPlanet(100, sunSettings);
        sun.planetRadius = Random.Range(70f, 100f);
        sun.GeneratePlanet();
        sunObject.transform.position = transform.position;
        Instantiate(sunLight, sun.transform);
        orbitingPlanets = Random.Range(1, settings.Length);
        ShuffleSettings();

        Vector3 position = sunObject.transform.position;
        position.x += (Random.Range(5, 15));

        
        for (int i = 0; i < orbitingPlanets; i++)
        {
            GameObject orbitPoint = Instantiate(rotator, sunObject.transform.position, Quaternion.identity, sunObject.transform);
            
            GameObject planetObj = new GameObject("Planet " + i);
            planetObj.transform.parent = orbitPoint.transform;
            CelestialBody planet = planetObj.AddComponent<Planet>();
            
            planet.SetupPlanet(100, settings[i]);
            planet.planetRadius = Random.Range(30f, 60f);
            planet.GeneratePlanet();
            position.x += Random.Range(15f, 25f);
            planetObj.transform.position = position;
        }

    }

    void ShuffleSettings()
    {
        PlanetSettings tempSet;
        for (int i = 0; i < settings.Length - 1; i++)
        {
            int rnd = Random.Range(i, settings.Length);
            tempSet = settings[rnd];
            settings[rnd] = settings[i];
            settings[i] = tempSet;
        }
    }


}
