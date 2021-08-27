using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInputControllerMousePublisher : MonoBehaviour {

    private ShipInputController inputController;
    [SerializeField] private Camera camera;

    private Vector3 mousePos = Vector3.zero;
    // Use this for initialization
    void Start () {
        inputController = GetComponent<ShipInputController>();
    }
	
    // Update is called once per frame
    void Update ()
    {
        Vector3 shipLocation = this.transform.position;

        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        float rayIterationCount = camera.transform.position.z / -cameraRay.direction.z;

        Vector3 planeSpaceMouse = new Vector3(cameraRay.origin.x + cameraRay.direction.x * rayIterationCount,
            cameraRay.origin.y + cameraRay.direction.y * rayIterationCount, 0);
        mousePos = planeSpaceMouse;

        Vector3 direction = (planeSpaceMouse - shipLocation);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        Debug.Log(direction);
        inputController.horizontal = direction.x;
        inputController.vertical = direction.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mousePos, .5f);
    }
}