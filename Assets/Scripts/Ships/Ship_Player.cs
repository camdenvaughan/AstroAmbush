using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ship_Player : Ship_Base
{
    [SerializeField] private Camera cam;


    private Vector3 mousePos;
    private Vector3 worldPoint;



    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10;
        worldPoint = cam.ScreenToWorldPoint(mousePos);
        RotateTowards(worldPoint);
    }

    private void FixedUpdate()
    {
        MoveTo(worldPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(worldPoint, 1);
    }
}
