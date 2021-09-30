using UnityEngine;

public class EnemyShip : ShipBase
{
    [SerializeField] private float shootCooldown;
    private float currentTime;

    protected override void SetDependencies()
    {
        base.SetDependencies();
        activeController = GetComponent<EnemyInputController>();
        currentTime = shootCooldown;
    }

    protected override void Shoot()
    {
        currentTime -= Time.deltaTime;
        if (!activeController.fire || currentTime > 0) return;
        int gunToggle = shootFromLeft ? 0 : 1;
        GameObject obj = ObjectPooler.GetEnemyBullet();
        obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
        obj.SetActive(true);
        gunFlare[gunToggle].Play();
        audioManager.Play("blaster");
        shootFromLeft = !shootFromLeft;
        currentTime = shootCooldown;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.GetState() == GameManager.GameState.Active ||
            GameManager.GetState() == GameManager.GameState.Tutorial)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                Explode();
                GameManager.AddToScore(5f);
            }
        }

    }

    private void Explode()
    {
        audioManager.Play("explosion");
        GameObject obj = ObjectPooler.GetExplosionObj();
        obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        obj.SetActive(true);
        gameObject.SetActive(false);
    }
}
