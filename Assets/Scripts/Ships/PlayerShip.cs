using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShip : ShipBase
{
    private KeyboardInputController keyboardController;
    private MouseInputController mouseController;

    [SerializeField] private int shotsBeforeCooldown;
    [SerializeField] private float consecutiveShotTime;
    [SerializeField] private float cooldownWaitTime;
    
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject shipMesh;
    [SerializeField] private float alienSpawnTimer;

    [SerializeField] private ParticleSystem hitEffect;

    [SerializeField] private int healthPoints;
    
    private float currentTime;
    private float coolDownTime;

    private int shots;
    private bool isOnCoolDown = false;
    private float consecutiveShotTimer;
    protected override void SetDependencies()
    {
        base.SetDependencies();
        mouseController = gameObject.AddComponent<MouseInputController>();
        keyboardController = gameObject.AddComponent<KeyboardInputController>();
        SetControls();
        uiNav.PauseStateChanged += OnPauseStateChanged;
        currentTime = alienSpawnTimer;
    }

    protected override void HandleActions()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            Move();
            SpawnAliens();
            Shoot();
            return;
        }

        anim.SetFloat("rotation", 0);
        if (GameManager.GetState() == GameManager.GameState.WaitingForInput)
            if (activeController.fire)
            {
                GameManager.SetGameToActive();
            }
    }

    private void CoolDownTimer()
    {
        consecutiveShotTimer += Time.deltaTime;

        if (!activeController.fire) return;
            
        if (consecutiveShotTimer < consecutiveShotTime)
        {
            shots++;
        }
        else
        {
            shots = 0;
        }
        consecutiveShotTimer = 0f;

        if (shots > shotsBeforeCooldown)
        {
            isOnCoolDown = true;
            shots = 0;
        }
    }

    protected override void Shoot()
    {
        if (isOnCoolDown)
        {
            coolDownTime += Time.deltaTime;
            if (coolDownTime > cooldownWaitTime)
            {
                coolDownTime = 0f;
                isOnCoolDown = false;
            }
            else
                return;
        }
        base.Shoot();
        CoolDownTimer();
    }

    void SpawnAliens()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            GameObject alienShip = ObjectPooler.GetAlienShip();
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            alienShip.transform.position = spawnPoints[randomSpawnPoint].transform.position;
            alienShip.SetActive(true);
            currentTime = alienSpawnTimer;
        }
    }

    void SetControls()
    {
        if (PlayerPrefs.GetInt("controlLayout", 0) == 0)
            activeController = mouseController;
        else
            activeController = keyboardController;
    }
    
    private void OnPauseStateChanged(object source, EventArgs e)
    {
        SetControls();
        GameManager.PauseGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
        else if (other.CompareTag("Bullet"))
        {
            if (healthPoints > 1)
            {
                healthPoints--;
                // Play Sound
                // Animate
                hitEffect.Play();
            }
            else
                Explode();
        }
        
    }

    private void Explode()
    {
        GameObject obj = ObjectPooler.GetExplosionObj();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        GameManager.EndGame();
        // play explosion
        audioManager.Play("explosion");
        shipMesh.SetActive(false);
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particleSystems)
        {
            system.Stop();
        }
    }
}
