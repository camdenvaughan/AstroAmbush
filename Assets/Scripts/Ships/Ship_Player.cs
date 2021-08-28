using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ship_Player : Ship_Base
{
    private Gun gun;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
        gun = GetComponentInChildren<Gun>();
    }

    private void Update()
    {
        if (inputController.fire)
            gun.Fire();
    }
}
