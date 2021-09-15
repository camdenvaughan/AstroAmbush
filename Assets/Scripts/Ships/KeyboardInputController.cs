using UnityEngine;

public class KeyboardInputController : ShipInputController
{
    private float cachedHorizontal = 1;
    private float cachedVertical = 1;
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            horizontal = cachedHorizontal;
            vertical = cachedVertical;
        }
        else
        {
            cachedHorizontal = horizontal;
            cachedVertical = vertical;
        }

        fire = Input.GetKeyDown(KeyCode.Space);
        rotate = true;
    }
}
