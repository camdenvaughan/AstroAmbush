using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    private ShipMovementController movement;
    private ShipInputController activeController;

    private void Start()
    {
        movement = GetComponent<ShipMovementController>();
        activeController = gameObject.AddComponent<MouseInputController>();
    }

    private void Update()
    {
        movement.Move(activeController.horizontal, activeController.vertical, activeController.rotate);
    }
}
