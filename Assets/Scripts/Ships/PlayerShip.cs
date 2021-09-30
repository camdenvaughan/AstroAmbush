using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class PlayerShip : ShipBase
{
    // Control Schemes
    private KeyboardInputController keyboardController;
    private MouseInputController mouseController;
    
    [Header("Ship Stats")]
    [SerializeField] private int maxHealthPoints;
    private int currentHealth;
    
    [Header("Blaster Cooldown Stats")]
    [SerializeField] private float overheatAmount;
    [SerializeField] private float heatPerShot;
    [SerializeField] private float beforeOverheatMultiplier;
    private float cooldownTimer;
    private bool isOnCoolDown = false;
    
    [Header("Alien Spawning")]
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float alienStartSpawnRate;
    [SerializeField] private float spawnRateDecreaseAmount;

    private float currentAlienSpawnTimer;

    [Header("Graphics Objects")]
    [SerializeField] private GameObject shipMesh;
    [SerializeField] private ParticleSystem[] particleSystems;
    
    [Header("Tutorial Stats and Objects")]
    [SerializeField] private float tutorialFlyTimer = 5f;
    [SerializeField] private int maxTutorialShots = 6;
    [SerializeField] private float maxTutorialAvoidTimer;
    private float tutorialAvoidTimer;
    private int tutorialShots;
    private bool hasSpawnedTutorialShips = false;
    private GameObject[] tutorialEnemies = new GameObject[2];

    private bool isInvincible = false;
    
    protected override void SetDependencies()
    {
        base.SetDependencies();
        mouseController = gameObject.AddComponent<MouseInputController>();
        keyboardController = gameObject.AddComponent<KeyboardInputController>();
        SetControls();
        uiNav.PauseStateChanged += OnPauseStateChanged;

        currentHealth = maxHealthPoints;
        
        currentAlienSpawnTimer = alienStartSpawnRate;

        tutorialAvoidTimer = maxTutorialAvoidTimer;

        uiNav.InitCoolDownBar();
        uiNav.InitHealthBar();
        GetComponentInChildren<InputField>().text = PlayerPrefs.GetString("displayName", "NEW");
    }

    protected override void HandleActions()
    {
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            Move();
            SpawnAliensOnTimer();
            Shoot();
            return;
        }
        
        if (GameManager.GetState() == GameManager.GameState.Tutorial)
        {
            switch (GameManager.GetTutorialStage())
            {
                case 0:
                    Stage0();
                    return;
                case 1:
                    Stage1();
                    return;
                case 2:
                    Stage2();
                    return;
                case 3:
                    Stage3();
                    return;
            }
        }

        anim.SetFloat("rotation", 0);
        if (GameManager.GetState() == GameManager.GameState.WaitingForInput)
            if (activeController.fire)
            {
                GameManager.SetGameToActive();
            }
    }
    
    protected override void Shoot()
    {
        if (isOnCoolDown)
        {
            cooldownTimer -= Time.deltaTime;
            uiNav.SetCoolDownBar(cooldownTimer, overheatAmount);
            if (cooldownTimer < 0)
            {
                isOnCoolDown = false;
            }
            else return;
        }
        base.Shoot();
        CoolDownTimer();
    }

    private void CoolDownTimer()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime * beforeOverheatMultiplier;
        if (activeController.fire)
            cooldownTimer += heatPerShot;
        if (cooldownTimer > overheatAmount)
        {
            isOnCoolDown = true;
            cooldownTimer = overheatAmount;
        }
        
        uiNav.SetCoolDownBar(cooldownTimer, overheatAmount);
    }


    private void SpawnAliensOnTimer()
    {
        currentAlienSpawnTimer -= Time.deltaTime;
        if (currentAlienSpawnTimer < 0)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            SpawnAliens(randomSpawnPoint);
            if (alienStartSpawnRate > 2f)
                alienStartSpawnRate -= spawnRateDecreaseAmount;
            currentAlienSpawnTimer = alienStartSpawnRate;
        }
    }

    private GameObject SpawnAliens(int spawnPoint)
    {
        GameObject alienShip = ObjectPooler.GetAlienShip();
        alienShip.transform.position = spawnPoints[spawnPoint].transform.position;
        alienShip.SetActive(true);
        return alienShip;
    }

    private void SetControls()
    {
        if (PlayerPrefs.GetInt("controlLayout", 0) == 0)
            activeController = mouseController;
        else
            activeController = keyboardController;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;
        if (GameManager.GetState() == GameManager.GameState.Active)
        {
            if (other.CompareTag("Enemy"))
            {
                Explode();
            }
            else if (other.CompareTag("Bullet"))
            {
                if (currentHealth > 1)
                {
                    currentHealth--;
                    // Play Sound
                    // Animate
                    anim.SetTrigger("hit");
                    isInvincible = true;
                }
                else
                    Explode();
                uiNav.SetHealthBar(currentHealth, maxHealthPoints);
            }
        }
        else if (GameManager.GetState() == GameManager.GameState.Tutorial)
        {
            if (other.CompareTag("Enemy"))
            {
                ResetStage();
            }
            else if (other.CompareTag("Bullet"))
            {
                if (currentHealth > 1)
                {
                    currentHealth--;
                    // Play Sound
                    // Animate
                    anim.SetTrigger("hit");
                }
                else
                {
                    ResetStage();
                }
                uiNav.SetHealthBar(currentHealth, maxHealthPoints);
            }
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

        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }

    private void TurnOffInvinsibility()
    {
        isInvincible = false;
    }
    
    
    private void OnPauseStateChanged(object source, EventArgs e)
    {
        SetControls();
        GameManager.PauseGame();
    }

    // Tutorial Functions

    private void Stage0() // Just Fly
    {
        tutorialFlyTimer -= Time.deltaTime;
        Move();
        if (tutorialFlyTimer < 0)
        {
            GameManager.IncrementTutorialStage();
        }
    }

    private void Stage1() // Just Shoot
    {
        Shoot();
        if (activeController.fire && !isOnCoolDown)
            tutorialShots++;
        
        if (tutorialShots > maxTutorialShots)
            GameManager.IncrementTutorialStage();
    }

    private void Stage2()
    {
        Move();
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime * beforeOverheatMultiplier;
            uiNav.SetCoolDownBar(cooldownTimer, overheatAmount);
        }
        
        if (!hasSpawnedTutorialShips)
        {
            tutorialEnemies[0] = SpawnAliens(2);
            tutorialEnemies[1] = SpawnAliens(4);
            hasSpawnedTutorialShips = true;
        }

        tutorialAvoidTimer -= Time.deltaTime;
        if (tutorialAvoidTimer < 0)
        {
            GameManager.IncrementTutorialStage();
        }
    }

    private void Stage3()
    {
        Move();
        Shoot();

        int i = 0;
        foreach (GameObject enemy in tutorialEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                i++;
                continue;
            }

            float dist = Vector3.Distance(enemy.transform.position, transform.position);

            if (dist > 100f)
            {
                int rand = Random.Range(0, spawnPoints.Length);
                enemy.transform.position = spawnPoints[rand].transform.position;
            }
        }
        if (i > 1)
            GameManager.EndTutorial();
    }

    private void ResetStage()
    {
        if (tutorialEnemies != null)
        {
            tutorialEnemies[0].transform.position = spawnPoints[2].transform.position;
            tutorialEnemies[1].transform.position = spawnPoints[4].transform.position;
        }

        tutorialAvoidTimer = maxTutorialAvoidTimer;

        Bullet[] bullets = FindObjectsOfType<Bullet>();

        foreach (Bullet bullet in bullets)
        {
            if (bullet.gameObject.activeInHierarchy)
                bullet.gameObject.SetActive(false);
        }
        GameManager.SetState(GameManager.GameState.WaitingForInput);
        string text = "Your Ship Exploded! ";
        text += PlayerPrefs.GetInt("controlLayout", 0) == 0 ? "Click Mouse Button " : "Press Space Bar ";
        text += "to Try Again!";
        uiNav.SetTopTutorialText(text, true);
        uiNav.SetBottomTutorialText("", true);
        currentHealth = maxHealthPoints;
        uiNav.SetHealthBar(currentHealth, maxHealthPoints);
    }
}
