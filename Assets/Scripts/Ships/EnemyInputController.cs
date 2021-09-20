
using UnityEngine;


public class EnemyInputController : ShipInputController
{
    [SerializeField] private float shootDistance;
    private void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 shipPos = GameManager.GetShipPos();

        Vector3 direction = (shipPos - currentPos).normalized;
        float dist = Vector3.Distance(currentPos, shipPos);

        horizontal = direction.x;
        vertical = direction.y;
        fire = dist < shootDistance;
    }
}
