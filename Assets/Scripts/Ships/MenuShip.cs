
public class MenuShip : ShipBase
{
    private void Start()
    {
        movement = GetComponent<ShipMovementController>();
        activeController = gameObject.AddComponent<MouseInputController>();
    }

    protected override void HandleActions()
    {
        Move();
    }
}
