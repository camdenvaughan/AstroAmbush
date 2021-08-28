using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ship_Player : Ship_Base
{

    [SerializeField] private Transform[] guns = new Transform[2];
    private bool shootFromLeft;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        if(inputController.fire)
        {
            int gunToggle = shootFromLeft ? 0 : 1;
            GameObject obj = ObjectPooler.GetBullet();
            obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
            obj.SetActive(true);
            shootFromLeft = !shootFromLeft;
        }
    }
}
