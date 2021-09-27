using UnityEngine;
using UnityEngine.UI;


public class ShipBase : MonoBehaviour
{
    protected ShipInputController activeController;
    protected ShipMovementController movement;
    
    protected Animator anim;
    
    [SerializeField] protected Transform[] guns;
    protected bool shootFromLeft;
    [SerializeField] protected ParticleSystem[] gunFlare;
    

    protected UINavigator uiNav;
    protected AudioManager audioManager;

    private void Start()
    {
        SetDependencies();
    }

    protected virtual void SetDependencies()
    {
        movement = GetComponent<ShipMovementController>();
        anim = GetComponent<Animator>();

        uiNav = FindObjectOfType<UINavigator>();
        audioManager = uiNav.audioManager;
    }

    private void Update()
    {
        HandleActions();
    }

    protected void Move()
    {
        movement.Move(activeController.horizontal, activeController.vertical);
    }

    protected virtual void HandleActions()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            Move();

            Shoot();
            return;
        }
        anim.SetFloat("rotation", 0);
    }

    protected virtual void Shoot()
    {
        if (!activeController.fire) return;

        int gunToggle = shootFromLeft ? 0 : 1;
        GameObject obj = ObjectPooler.GetPlayerBullet();
        obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
        obj.SetActive(true);
        gunFlare[gunToggle].Play();
        audioManager.Play("blaster");
        shootFromLeft = !shootFromLeft;
    }

    public void UpdatedName(string name)
    {
        if (name != string.Empty)
            PlayerPrefs.SetString("displayName", name);
        else
            GetComponentInChildren<InputField>().text = PlayerPrefs.GetString("displayName", "NEW");

        uiNav.GetComponent<PlayFabManager>().UpdateDisplayName();
    }
}
