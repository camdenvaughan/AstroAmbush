
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler current;
    [Header("Bullets")]
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private int bulletAmmount;
    [Header("Planet Debris")]
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private int explosionAmmount;
    [Header("Suns")]
    [SerializeField] private SunSettings sunSettings;
    [SerializeField] private GameObject sunLight;
    [SerializeField] private int sunAmmount;
    [Header("Planets")]
    [SerializeField] private PlanetSettings[] planetSettings;
    [SerializeField] private int planetAmmount;


    private List<GameObject> pooledBullets = new List<GameObject>();
    private List<GameObject> pooledExplosionObjs = new List<GameObject>();
    private List<GameObject> pooledSuns = new List<GameObject>();
    private List<GameObject> pooledPlanets = new List<GameObject>();

    private void Start()
    {
        current = this;
        for (int i = 0; i < bulletAmmount; i++)
        {
            pooledBullets.Add(Instantiate(bulletObj, transform));
            pooledBullets[i].SetActive(false);
        }

        for (int i = 0; i < explosionAmmount; i++)
        {
           pooledExplosionObjs.Add(Instantiate(explosionObj, transform)); 
           pooledExplosionObjs[i].SetActive(false);
        }

        for (int i = 0; i < sunAmmount; i++)
        {
            GameObject sunObj = CreateSun();
            sunObj.SetActive(false);
            pooledSuns.Add(sunObj);
        }

        for (int i = 0; i < planetAmmount; i++)
        {
            GameObject planetObj = CreatePlanet();
            planetObj.SetActive(false);
            pooledPlanets.Add(planetObj);
        }
        
    }
    public static GameObject GetBullet()
    {
        return current.GetBulletImpl();
    }
    
    private GameObject GetBulletImpl()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
                return pooledBullets[i];
        }
        GameObject obj = Instantiate(bulletObj, transform);
        pooledBullets.Add(obj);
        return obj;
    }

    public static GameObject GetExplosionObj()
    {
        return current.GetExplosionObjImpl();
    }

    private GameObject GetExplosionObjImpl()
    {
        for (int i = 0; i < pooledExplosionObjs.Count; i++)
        {
            if (!pooledExplosionObjs[i].activeInHierarchy)
                return pooledExplosionObjs[i];
        }

        GameObject obj = Instantiate(explosionObj, transform);
        pooledExplosionObjs.Add(obj);
        return obj;
    }

    public static GameObject GetSun()
    {
        return current.GetSunImpl();
    }
    private GameObject GetSunImpl()
    {
        for (int i = 0; i < pooledSuns.Count; i++)
        {
            if (!pooledSuns[i].activeInHierarchy)
                return pooledSuns[i];
        }

        GameObject obj = CreateSun();
        pooledSuns.Add(obj);
        return obj;
    }

    public static GameObject GetPlanet()
    {
        return current.GetPlanetImpl();
    }

    private GameObject GetPlanetImpl()
    {
        for (int i = 0; i < pooledPlanets.Count; i++)
        {
            if (!pooledPlanets[i].activeInHierarchy)
                return pooledPlanets[i];
        }

        GameObject obj = CreatePlanet();
        pooledPlanets.Add(obj);
        return obj;
    }

    private GameObject CreateSun()
    {
        GameObject sunObj = new GameObject("Sun");
        sunObj.transform.parent = transform;
        Sun sun = sunObj.AddComponent<Sun>();
        sun.SetupPlanet(50, sunSettings);
        sun.planetRadius = Random.Range(70f, 100f);
        sun.GeneratePlanet();
        Instantiate(sunLight, sunObj.transform);
        return sunObj;
    }

    private GameObject CreatePlanet()
    {
        GameObject planetObj = new GameObject("Planet");
        planetObj.transform.parent = transform;
        Planet planet = planetObj.AddComponent<Planet>();
        int rnd = Random.Range(0, planetSettings.Length);
        planet.SetupPlanet(75, planetSettings[rnd]);
        planet.planetRadius = Random.Range(30f, 60f);
        planet.GeneratePlanet();
        return planetObj;
    }

    public static void DestroyPlanet(GameObject planet)
    {
        current.DestroyPlanetImpl(planet);
        
    }

    private void DestroyPlanetImpl(GameObject planet)
    {
        planet.SetActive(false);
        planet.transform.parent = transform;
    }
    

}
