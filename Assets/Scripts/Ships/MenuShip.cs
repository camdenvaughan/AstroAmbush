using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    private ShipMovementController movement;

    private void Start()
    {
        movement = GetComponent<ShipMovementController>();
    }

    private void Update()
    {
        movement.Move();
    }
}
