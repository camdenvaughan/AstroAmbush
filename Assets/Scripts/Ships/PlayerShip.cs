using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class PlayerShip : ShipBase
{
    private KeyboardInputController keyboardController;
    private MouseInputController mouseController;

    [SerializeField] private int shotsBeforeCooldown;
    [SerializeField] private float consecutiveShotTime;
    [SerializeField] private float cooldownWaitTime;
    [SerializeField] private float currentShotTimeMultiplier;
    
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject shipMesh;
    [SerializeField] private float alienSpawnTimer;

    [SerializeField] private int healthPoints;
    
    private float currentTime;
    private float coolDownTime;

    private float currentShotsTimer = 0;

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
        coolDownTime = 0;
        uiNav.InitCoolDownBar();
        uiNav.InitHealthBar(healthPoints);
        GetComponentInChildren<InputField>().text = PlayerPrefs.GetString("displayName", "NEW");

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
        if (currentShotsTimer > 0)
            currentShotsTimer -= Time.deltaTime * currentShotTimeMultiplier;
        else
            currentShotsTimer = 0;

        if (activeController.fire)
        {
            if (consecutiveShotTimer < consecutiveShotTime)
            {
                shots++;
            }
            else
            {
                shots = 1;
            }
            consecutiveShotTimer = 0f;
            
            float i = (float)shots / ((float)shotsBeforeCooldown + 1);
            currentShotsTimer = cooldownWaitTime * i;

            if (shots > shotsBeforeCooldown)
            {
                isOnCoolDown = true;
                coolDownTime = cooldownWaitTime;
                shots = 0;
                currentShotsTimer = 0;
            }
        }
        
        uiNav.SetCoolDownBar(currentShotsTimer, cooldownWaitTime);
    }

    protected override void Shoot()
    {


        if (isOnCoolDown)
        {
            coolDownTime -= Time.deltaTime;
            uiNav.SetCoolDownBar(coolDownTime, cooldownWaitTime);
            if (coolDownTime < 0)
            {
                coolDownTime = cooldownWaitTime;
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
                anim.SetTrigger("hit");
            }
            else
                Explode();
            uiNav.SetHealthBar(healthPoints);
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
        Destroy(shipMesh);
        ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in particleSystems)
        {
            system.Stop();
        }
    }
}
