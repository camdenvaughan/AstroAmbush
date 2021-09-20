using UnityEngine;

public class NoiseFilter
{
    private PlanetSettings.NoiseLayer settings;
    private Noise noise = new Noise();

    public NoiseFilter(PlanetSettings.NoiseLayer settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        switch (settings.filter)
        {
            case PlanetSettings.FilterType.Simple:
                for (int i = 0; i < settings.numLayers; i++)
                {
                    float v = noise.Evaluate(point * frequency + settings.center);
                    noiseValue += (v + 1) * .5f * amplitude;
                    frequency *= settings.roughness;
                    amplitude *= settings.persistence;
                }

                noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
                return noiseValue * settings.strength;

            case PlanetSettings.FilterType.Rigid:
                float weight = 1;
                for (int i = 0; i < settings.numLayers; i++)
                {
                    float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
                    v *= v;
                    v *= weight;
                    weight = Mathf.Clamp01(v * settings.weightMultiplier);

                    noiseValue += v * amplitude;
                    frequency *= settings.roughness;
                    amplitude *= settings.persistence;
                }

                noiseValue = Mathf.Max(0, noiseValue - settings.minValue); 
                return noiseValue * settings.strength;
            default:
                return 0;
        }

        /*float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;*/
    }
}
