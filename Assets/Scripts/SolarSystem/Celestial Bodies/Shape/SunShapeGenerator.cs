using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShapeGenerator : IShapeGenerator
{
    private SunSettings settings;
    private float planetRadius;
    public void UpdateSettings(CelestialSettings settings, float planetRadius)
    {
        this.settings = (SunSettings)settings;
        this.planetRadius = planetRadius;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * planetRadius / 10f;
    }
}
