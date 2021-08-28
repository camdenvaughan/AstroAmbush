using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Bullet[] bullets;
    private int bulletIterator = 0;
    private void Start()
    {
        bullets = new Bullet[100];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bullet).GetComponent<Bullet>();
        }
    }

    public void Fire()
    {
        bullets[bulletIterator].transform.position = this.transform.position;
        bullets[bulletIterator].Fire();
        bulletIterator++;
    }
}
