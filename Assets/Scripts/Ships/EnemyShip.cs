using UnityEngine;

public class EnemyShip : ShipBase
{
    [SerializeField] private float shootCooldown;
    private float startTime;

    protected override void SetDependencies()
    {
        base.SetDependencies();
        activeController = GetComponent<EnemyInputController>();
        startTime = Time.time;
    }

    protected override void Shoot()
    {
        if (!activeController.fire || Time.time - startTime < shootCooldown) return;
        int gunToggle = shootFromLeft ? 0 : 1;
        GameObject obj = ObjectPooler.GetEnemyBullet();
        obj.transform.SetPositionAndRotation(guns[gunToggle].position, guns[gunToggle].rotation);
        obj.SetActive(true);
        gunFlare[gunToggle].Play();
        audioManager.Play("blaster");
        shootFromLeft = !shootFromLeft;
        startTime = Time.time;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Explode();
            audioManager.Play("explosion");
            gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        GameObject obj = ObjectPooler.GetExplosionObj();
        obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        obj.SetActive(true);
        
    }
}
