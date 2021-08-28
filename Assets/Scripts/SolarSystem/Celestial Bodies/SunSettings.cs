using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SunSettings : CelestialSettings
{
    public SunSettings(SunSettings settings)
    {
        planetRadius = settings.planetRadius;
        planetShader = settings.planetShader;
    }
}
