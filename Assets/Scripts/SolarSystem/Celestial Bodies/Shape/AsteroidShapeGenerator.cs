using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShapeGenerator : IShapeGenerator
{
    private PlanetSettings settings;
    private NoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    private float planetRadius;
    public void UpdateSettings(CelestialSettings settings, float planetRadius)
    {
        this.settings = (PlanetSettings)settings;
        this.planetRadius = planetRadius;
        noiseFilters = new NoiseFilter[this.settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = new NoiseFilter(this.settings.noiseLayers[i]);
        }

        elevationMinMax = new MinMax();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = (planetRadius / 10) * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}
