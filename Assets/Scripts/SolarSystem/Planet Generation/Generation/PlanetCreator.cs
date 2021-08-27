using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetCreator : MonoBehaviour
{
    [SerializeField] private GameObject rotator;
    [SerializeField] private PlanetSettings[] settings;

    private int orbitingPlanets;
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Sun");
        Planet sun = sunObject.AddComponent<Planet>();
        sun.SetupPlanet(100, settings[0]);
        sun.GeneratePlanet();

        orbitingPlanets = settings.Length;

        Vector3 position = sunObject.transform.position;
        for (int i = 1; i < orbitingPlanets; i++)
        {
            GameObject orbitPoint = Instantiate(rotator, sunObject.transform.position, Quaternion.identity);
            
            GameObject planetObj = new GameObject("Planet " + i);
            planetObj.transform.parent = orbitPoint.transform;
            Planet planet = planetObj.AddComponent<Planet>();
            
            planet.SetupPlanet(100, settings[i]);
            planet.GeneratePlanet();
            
            position.x += (Random.Range(15, 30) * i);
            planetObj.transform.position = position;
        }
    }


}
