using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class PlanetSettings : CelestialSettings
{
    [Header("Color Settings")]
    public Gradient gradient;
    
    [HideInInspector]
    public Material planetMaterial;

    [Header("Noise Settings")]
    public NoiseLayer[] noiseLayers;
    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        [Range(-5,5)]
        public float strength = 1;
        [Range(1,8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public Vector3 center;
        public float minValue;
    }
}
