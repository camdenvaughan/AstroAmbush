using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler current;
    [Header("Bullets")]
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private int bulletAmmount;
    [Header("Planet Debris")]
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private int explosionAmmount;
    [Header("Aliens")] 
    [SerializeField] private GameObject alienShip;
    [SerializeField] private int alienAmount;

    private List<GameObject> pooledPlayerBullets = new List<GameObject>();
    private List<GameObject> pooledEnemyBullets = new List<GameObject>();
    private List<GameObject> pooledExplosionObjs = new List<GameObject>();
    private List<GameObject> pooledAliens = new List<GameObject>();
    private List<GameObject> pooledAsteroids = new List<GameObject>();


    private void Start()
    {
        current = this;
        for (int i = 0; i < bulletAmmount; i++)
        {
            pooledPlayerBullets.Add(Instantiate(playerBullet, transform));
            pooledPlayerBullets[i].SetActive(false);
        }
        
        for (int i = 0; i < bulletAmmount; i++)
        {
            pooledEnemyBullets.Add(Instantiate(enemyBullet, transform));
            pooledEnemyBullets[i].SetActive(false);
        }

        for (int i = 0; i < explosionAmmount; i++)
        {
           pooledExplosionObjs.Add(Instantiate(explosionObj, transform)); 
           pooledExplosionObjs[i].SetActive(false);
        }

        for (int i = 0; i < alienAmount; i++)
        {
            pooledAliens.Add(Instantiate(alienShip, transform));
            pooledAliens[i].SetActive(false);
        }
    }
    public static GameObject GetPlayerBullet()
    {
        for (int i = 0; i < current.pooledPlayerBullets.Count; i++)
        {
            if (!current.pooledPlayerBullets[i].activeInHierarchy)
                return current.pooledPlayerBullets[i];
        }
        GameObject obj = Instantiate(current.playerBullet, current.transform);
        current.pooledPlayerBullets.Add(obj);
        return obj;
    }
    
    public static GameObject GetEnemyBullet()
    {
        for (int i = 0; i < current.pooledEnemyBullets.Count; i++)
        {
            if (!current.pooledEnemyBullets[i].activeInHierarchy)
                return current.pooledEnemyBullets[i];
        }
        GameObject obj = Instantiate(current.enemyBullet, current.transform);
        current.pooledEnemyBullets.Add(obj);
        return obj;
    }

    public static GameObject GetExplosionObj()
    {
        for (int i = 0; i < current.pooledExplosionObjs.Count; i++)
        {
            if (!current.pooledExplosionObjs[i].activeInHierarchy)
                return current.pooledExplosionObjs[i];
        }

        GameObject obj = Instantiate(current.explosionObj, current.transform);
        current.pooledExplosionObjs.Add(obj);
        return obj;
    }

    public static GameObject GetAlienShip()
    {
        for (int i = 0; i < current.pooledAliens.Count; i++)
        {
            if (!current.pooledAliens[i].activeInHierarchy)
                return current.pooledAliens[i];
        }

        GameObject obj = Instantiate(current.alienShip, current.transform);
        current.pooledAliens.Add(obj);
        return obj;
    }
}
