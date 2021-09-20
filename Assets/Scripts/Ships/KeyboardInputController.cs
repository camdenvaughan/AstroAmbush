using UnityEngine;

public class KeyboardInputController : ShipInputController
{
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        fire = Input.GetKeyDown(KeyCode.Space);
    }
}
