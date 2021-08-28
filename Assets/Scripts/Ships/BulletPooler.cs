using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private int pooledAmmount;
    private int poolIterator = 0;

    private List<GameObject> pooledBullets;

    private void Start()
    {
        pooledBullets = new List<GameObject>();
        for (int i = 0; i < pooledAmmount; i++)
        {
            pooledBullets.Add(Instantiate(bulletObj));
            pooledBullets[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                Debug.Log("Got a bullet");
                return pooledBullets[i];
            }
        }
        
        GameObject obj = Instantiate(bulletObj);
        pooledBullets.Add(obj);
        return obj;
    }
}
