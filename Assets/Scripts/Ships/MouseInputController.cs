using UnityEngine;

public class MouseInputController : ShipInputController {

    private Camera cam;

    public enum ControlState
    {
        Mouse, Keyboard, Controller
    }

    private ControlState state;
    
    
    private Vector3 mousePos = Vector3.zero;
    // Use this for initialization

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 shipLocation = this.transform.position;

        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        float rayIterationCount = cam.transform.position.z / -cameraRay.direction.z;

        Vector3 planeSpaceMouse = new Vector3(cameraRay.origin.x + cameraRay.direction.x * rayIterationCount,
            cameraRay.origin.y + cameraRay.direction.y * rayIterationCount, 0);
        mousePos = planeSpaceMouse;

        Vector3 direction = (planeSpaceMouse - shipLocation);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        horizontal = direction.x;
        vertical = direction.y;

        fire = Input.GetMouseButtonDown(0);
    }

    public void ChangeControlState(ControlState changeState)
    {
        state = changeState;
    }

    void GatherMouseInput()
    {
        Vector3 shipLocation = this.transform.position;

        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        float rayIterationCount = cam.transform.position.z / -cameraRay.direction.z;

        Vector3 planeSpaceMouse = new Vector3(cameraRay.origin.x + cameraRay.direction.x * rayIterationCount,
            cameraRay.origin.y + cameraRay.direction.y * rayIterationCount, 0);
        mousePos = planeSpaceMouse;

        Vector3 direction = (planeSpaceMouse - shipLocation);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        horizontal = direction.x;
        vertical = direction.y;

        fire = Input.GetMouseButtonDown(0);
    }

    void GatherKeyBoardInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        fire = Input.GetKeyDown(KeyCode.Space);
    }
}