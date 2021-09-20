using UnityEngine;

public class MouseInputController : ShipInputController {

    private Camera cam;

    private Vector3 mousePos = Vector3.zero;
    // Use this for initialization
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update ()
    {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        float rayIterationCount = cam.transform.position.z / -cameraRay.direction.z;

        Vector3 planeSpaceMouse = new Vector3(cameraRay.origin.x + cameraRay.direction.x * rayIterationCount,
            cameraRay.origin.y + cameraRay.direction.y * rayIterationCount, 0);
        mousePos = planeSpaceMouse;

        Vector3 direction = (planeSpaceMouse - transform.position);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        horizontal = direction.x;
        vertical = direction.y;

        fire = Input.GetMouseButtonDown(0);

        float distance = Vector3.Distance(mousePos, transform.position);
    }
}