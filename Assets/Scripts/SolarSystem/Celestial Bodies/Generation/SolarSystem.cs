using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SolarSystem : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    [SerializeField] private SunSettings sunSettings;
    [SerializeField] private PlanetSettings[] settings;

    private int orbitingPlanets;
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Sun");
        CelestialBody sun = sunObject.AddComponent<Sun>();
        sun.SetupPlanet(100, sunSettings);
        sun.GeneratePlanet();
        orbitingPlanets = settings.Length;

        Vector3 position = sunObject.transform.position;
        position.x += (Random.Range(5, 15));
        
        for (int i = 0; i < orbitingPlanets; i++)
        {
            Debug.Log("Start of Loop");
            GameObject orbitPoint = Instantiate(rotator, sunObject.transform.position, Quaternion.identity);
            
            GameObject planetObj = new GameObject("Planet " + i);
            planetObj.transform.parent = orbitPoint.transform;
            CelestialBody planet = planetObj.AddComponent<Planet>();
            
            planet.SetupPlanet(100, settings[i]);
            Debug.Log("Bout to Generate");
            planet.GeneratePlanet();
            Debug.Log("Hello");
            position.x += (Random.Range(5, 15));
            planetObj.transform.position = position;
        }

    }


}
