using System.Collections;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public float maxX;
    public Transform spawnPoint;
    public float spawnRate;
    public float minSpawnRate = 0.5f;

    bool gameStarted = false;

    public TextMeshProUGUI scoreText;

    public Player player;

    public GameObject level;
    public UIManager uiManager;

    int score = 0;
    bool isPaused = false;
    bool isGameOver = false;


    [Header("Buffs")]
    public float buffSpawnChance = 0.2f;
    public GameObject buffPrefab;
    public float currentBuffHPMultiplier = 4f;
    public float buffHPScalingRate = 0.4f;

    [Header("Buff spawn settings")]
    public int doubleBulletsLimit = 1;
    public int damageUpLimit = 0;
    public int rapidFireLimit = 10;
    public int healLimit = -1;
    public int shadowCloneLimit = 0;
    public int freezeLimit = 0;

    [Header("Rewarded ad placeholder")]
    public float rewardedAdLoadingDuration = 5f;

    [Header("Ad removal placeholder")]
    public float adRemovalCheckoutLoadingDuration = 4f;

    [Header("Win condition")]
    public int targetScoreToWin = 10;

    int respawnCount = 0;
    bool isRespawnAdRunning = false;
    Coroutine respawnAdCoroutine;
    bool adsRemoved = false;
    bool isAdRemovalPurchaseRunning = false;
    Coroutine adRemovalPurchaseCoroutine;
    bool isLevelWon = false;
    bool isEndlessModeActive = false;


    // Update is called once per frame
    void Update()
    {
        if (isPaused || isGameOver || isLevelWon)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            gameStarted = true;
            StartSpawning();
        }
    }

    void OnDisable()
    {
        if (uiManager != null)
        {
            uiManager.SetLoadingSpinnerVisible(false);
        }

        if (isPaused)
        {
            ResumeGame();
        }
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            AttemptSpawnBuff();
            yield return new WaitForSeconds(spawnRate);
            spawnRate = Mathf.Max(minSpawnRate, spawnRate * 0.98f);
            currentBuffHPMultiplier += buffHPScalingRate;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = spawnPoint.position;

        spawnPos.x = Random.Range(-maxX, maxX);

        RegisterLevelObject(Instantiate(enemy, spawnPos, Quaternion.identity));
    }

    public void AttemptSpawnBuff()
    {
        if (Random.value < buffSpawnChance)
        {
            Vector3 spawnPos = spawnPoint.position;
            spawnPos.x = Random.Range(-maxX, maxX);
            SpawnRandomBuff(spawnPos);
        }
    }

    private void SpawnRandomBuff(Vector3 position)
    {
        System.Collections.Generic.List<Buff.BuffType> availableBuffs = new System.Collections.Generic.List<Buff.BuffType>();

        if (doubleBulletsLimit != 0) availableBuffs.Add(Buff.BuffType.DoubleBullets);
        if (damageUpLimit != 0) availableBuffs.Add(Buff.BuffType.DamageUp);
        if (rapidFireLimit != 0) availableBuffs.Add(Buff.BuffType.RapidFire);
        if (healLimit != 0 && player.hpBar.GetHP() < player.hpBar.hp_max) availableBuffs.Add(Buff.BuffType.Heal);
        if (shadowCloneLimit != 0) availableBuffs.Add(Buff.BuffType.ShadowClone);
        if (freezeLimit != 0) availableBuffs.Add(Buff.BuffType.Freeze);

        if (availableBuffs.Count == 0) return;

        Buff.BuffType selectedBuff = availableBuffs[Random.Range(0, availableBuffs.Count)];

        GameObject buffObj = RegisterLevelObject(Instantiate(buffPrefab, position, Quaternion.identity));
        Buff buff = buffObj.GetComponent<Buff>();
        if (buff != null)
        {
            buff.commonHPMultiplier = currentBuffHPMultiplier;
            buff.Init(selectedBuff);
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver || isLevelWon)
        {
            return;
        }

        score += points;

        scoreText.text = score.ToString();

        if (!isEndlessModeActive && score >= targetScoreToWin)
        {
            WinGame();
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void TogglePause()
    {
        if (isGameOver || isLevelWon)
        {
            return;
        }

        SetPaused(!isPaused);
    }

    public void ResumeGame()
    {
        SetPaused(false);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameFinished()
    {
        return isGameOver || isLevelWon;
    }

    public void GameOver()
    {
        if (isGameOver || isLevelWon)
        {
            return;
        }

        isGameOver = true;
        SetPaused(true);

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
    }

    public void WinGame()
    {
        if (isLevelWon || isGameOver)
        {
            return;
        }

        isLevelWon = true;
        SetPaused(true);

        if (uiManager != null)
        {
            uiManager.ShowWin();
        }
    }

    public void StartEndlessMode()
    {
        if (!isLevelWon)
        {
            return;
        }

        isLevelWon = false;
        isEndlessModeActive = true;
        ResumeGame();

        if (uiManager != null)
        {
            uiManager.ShowGameplay();
        }
    }

    public void RestartGame()
    {
        respawnCount = 0;
        isLevelWon = false;
        isEndlessModeActive = false;

        if (respawnAdCoroutine != null)
        {
            StopCoroutine(respawnAdCoroutine);
            respawnAdCoroutine = null;
        }

        if (adRemovalPurchaseCoroutine != null)
        {
            StopCoroutine(adRemovalPurchaseCoroutine);
            adRemovalPurchaseCoroutine = null;
        }

        isRespawnAdRunning = false;
        isAdRemovalPurchaseRunning = false;

        if (uiManager != null)
        {
            uiManager.UpdateRespawnCounter(respawnCount);
            uiManager.UpdateAdRemovalState(adsRemoved);
            uiManager.SetLoadingSpinnerVisible(false);
        }

        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BuyAdRemoval()
    {
        uiManager.ShowRespawnLoading();

        if (adsRemoved || isAdRemovalPurchaseRunning)
        {
            return;
        }
        //respawnAdCoroutine = StartCoroutine(PlayRespawnAdRoutine());
        //adRemovalPurchaseCoroutine = StartCoroutine(BuyAdRemovalRoutine());
    }

    public void CompleteAdRemovalPurchase()
    {
        adsRemoved = true;
        isAdRemovalPurchaseRunning = false;
        adRemovalPurchaseCoroutine = null;

        if (uiManager != null)
        {
            uiManager.SetLoadingSpinnerVisible(false);
            uiManager.UpdateAdRemovalState(adsRemoved);
        }
    }

    public void PlayRespawnAd()
    {
        if (!isGameOver || isRespawnAdRunning)
        {
            return;
        }

        respawnAdCoroutine = StartCoroutine(PlayRespawnAdRoutine());
    }

    public void RespawnPlayerAfterAd()
    {
        if (!isGameOver || player == null)
        {
            return;
        }

        isGameOver = false;
        isRespawnAdRunning = false;
        respawnAdCoroutine = null;
        ResumeGame();
        player.Respawn();
        respawnCount++;

        if (uiManager != null)
        {
            uiManager.UpdateRespawnCounter(respawnCount);
            uiManager.ShowGameplay();
        }
    }

    private IEnumerator PlayRespawnAdRoutine()
    {
        isRespawnAdRunning = true;

        if (uiManager != null)
        {
            uiManager.ShowRespawnLoading();
            yield return uiManager.ShowLoadingSpinnerForSeconds(rewardedAdLoadingDuration);
        }
        else
        {
            yield return new WaitForSecondsRealtime(rewardedAdLoadingDuration);
        }

        // Placeholder for rewarded ad load/play callbacks.
        RespawnPlayerAfterAd();
    }

    private IEnumerator BuyAdRemovalRoutine()
    {
        isAdRemovalPurchaseRunning = true;

        if (uiManager != null)
        {
            uiManager.SetLoadingSpinnerVisible(true);
        }

        yield return new WaitForSecondsRealtime(adRemovalCheckoutLoadingDuration);

        // Placeholder for checkout completion callback.
        CompleteAdRemovalPurchase();
    }

    public GameObject RegisterLevelObject(GameObject levelObject)
    {
        if (level != null && levelObject != null)
        {
            levelObject.transform.SetParent(level.transform, true);
        }

        return levelObject;
    }

    public void ActivateDoubleBullets()
    {
        if (doubleBulletsLimit > 0) doubleBulletsLimit--;
        player.doubleBullets = true;   
    }

    public void ActivateDamageUp()
    {
        if (damageUpLimit > 0) damageUpLimit--;
        player.flatDamageUp += 1;
    }

    public void ActivateRapidFire()
    {
        if (rapidFireLimit > 0) rapidFireLimit--;
        player.reloadTime *= 0.7f;
    }

    public void ActivateHeal()
    {
        if (healLimit > 0) healLimit--;
        player.Heal(20);
    }

    public void ActivateShadowClone()
    {
        if (shadowCloneLimit > 0) shadowCloneLimit--;
    }

    public void ActivateFreeze()
    {
        if (freezeLimit > 0) freezeLimit--;
    }

}
