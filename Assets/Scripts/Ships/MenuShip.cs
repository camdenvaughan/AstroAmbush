using UnityEngine;
using UnityEngine.UI;
public class MenuShip : ShipBase
{
    protected override void SetDependencies()
    {
        base.SetDependencies();
        activeController = gameObject.AddComponent<MouseInputController>();
        GetComponentInChildren<InputField>().text = PlayerPrefs.GetString("displayName", "NEW");
    }

    protected override void HandleActions()
    {
        Move();
    }
}
