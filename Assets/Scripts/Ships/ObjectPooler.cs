using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler current;
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private int bulletAmmount;
    [SerializeField] private GameObject explosionObj;
    [SerializeField] private int explosionAmmount;


    private List<GameObject> pooledBullets = new List<GameObject>();
    private List<GameObject> pooledExplosionObjs = new List<GameObject>();

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

}
