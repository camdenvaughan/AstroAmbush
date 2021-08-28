using UnityEngine;

public interface IShapeGenerator
{

    void UpdateSettings(CelestialSettings settings, float planetRadius);

    Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere);

}
