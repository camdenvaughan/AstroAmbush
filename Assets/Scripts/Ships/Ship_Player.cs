using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ship_Player : Ship_Base
{
    private BulletPooler bulletPooler;
    public bool fire;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
        bulletPooler = GetComponentInChildren<BulletPooler>();
    }

    private void Update()
    {
        if(inputController.fire)
        {
            Debug.Log("Firing");
            GameObject obj = bulletPooler.GetBullet();
            obj.transform.rotation = transform.rotation;
            obj.transform.position = transform.position;
            obj.SetActive(true);
            fire = false;
        }
    }
}
