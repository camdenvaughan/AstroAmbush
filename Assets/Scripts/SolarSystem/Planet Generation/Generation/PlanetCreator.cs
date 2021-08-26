using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetCreator : MonoBehaviour
{
    [SerializeField] private PlanetSettings[] settings;

    private int orbitingPlanets;
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Sun");
        Planet sun = sunObject.AddComponent<Planet>();
        sun.SetupPlanet(100, settings[0]);
        sun.GeneratePlanet();

        orbitingPlanets = Random.Range(1, 5);

        Vector3 position = sunObject.transform.position;
        for (int i = 1; i < orbitingPlanets + 1; i++)
        {
            GameObject planetObj = new GameObject("Planet " + i);

            Planet planet = planetObj.AddComponent<Planet>();
            planet.SetupPlanet(100, settings[i+1]);
            planet.GeneratePlanet();
            position.x += (50 * i);
            planetObj.transform.position = position;
        }
    }


}
