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
    
    void Start()
    {
        // Create Sun
        GameObject sunObject = new GameObject("Planet");
        Planet sun = sunObject.AddComponent<Planet>();
        sun.SetupPlanet(100, settings[0].shapeSettings, settings[0].colorSettings);
        sun.GeneratePlanet();
        
        // Create Settings
        // Create Planet with Settings
    }


}
