using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShapeGenerator : IShapeGenerator
{
    private SunSettings settings;
    public void UpdateSettings(CelestialSettings settings)
    {
        this.settings = (SunSettings)settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * settings.planetRadius;
    }
}
