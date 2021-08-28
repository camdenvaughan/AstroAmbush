using UnityEngine;

public interface IShapeGenerator
{

    void UpdateSettings(CelestialSettings settings);

    Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere);

}
