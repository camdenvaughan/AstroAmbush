using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetCreator : MonoBehaviour
{
    [System.Serializable]
    private class Settings
    {
        public ShapeSettings shapeSettings;
        public ColorSettings colorSettings;
    }

    [SerializeField] private Settings[] settings;

    private int orbitingPlanets;
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Sun");
        Planet sun = sunObject.AddComponent<Planet>();
        sun.SetupPlanet(100, settings[0].shapeSettings, settings[0].colorSettings);
        sun.GeneratePlanet();

        orbitingPlanets = Random.Range(1, 5);

        Vector3 position = sunObject.transform.position;
        for (int i = 1; i < orbitingPlanets + 1; i++)
        {
            GameObject planetObj = new GameObject("Planet " + i);

            Planet planet = planetObj.AddComponent<Planet>();
            planet.SetupPlanet(100, settings[i+1].shapeSettings, settings[i+1].colorSettings);
            planet.GeneratePlanet();
            position.x += (50 * i);
            planetObj.transform.position = position;
        }
    }


}
