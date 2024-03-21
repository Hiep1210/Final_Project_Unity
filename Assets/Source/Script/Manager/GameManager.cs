using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Setting Wave: ")]
    public int numberEnemyInWave;
    public float timeSpawnBetweenEnemy;
    public int indexWaveStartInitBoss = 1;
    public int numberBossInWave;
    public float timeSpawnBetweenBoss;

    [Header("Setting Multi Wave Index: ")]
    public int multiNumberEnemyInWave;
    public int multiNumberBossInWave;
    [Range(0f, 10f)]
    public float multiTimeSpawnBetweenEnemy;
    [Range(0f, 10f)]
    public float multiTimeSpawnBetweenBoss;

    [Header("Setting Multi Add Moster Normal Index: ")]
    public float multiToAddHp;
    public float multiToAddMoveSpeed;
    public float multiToAddChassingSpeed;
    public float multiToAddDame;
    public float multiAddAttackRate;
    public float multiAddFlySpeedBullet;

    [Header("Setting Pos Spawn Limit: ")]
    public float posXRightSpawnEnemyLimit;
    public float posXLeftPosSpawnEnemyLimit;
    public float posYUpPosSpawnEnemyLimit;
    public float posYDownPosSpawnEnemyLimit;

    [Header("Data For Wave: ")]
    public List<Actor> normalEnemyPrefabs;
    public List<Actor> bossNormalEnemyPrefabs;

    private Player m_player;

    private bool m_isStaringWave;
    private int m_currentIndexWave = 1;

    private float m_timePlaying;
    private float m_Score;
    private int m_countKillEnemy;
    private float m_coin;

    // SystemLevel;
    private int m_curlevel = 1;
    private float m_curExperessInLevels = 0;
    private float m_requireExpressToNextLevel = 200f;
    private float m_multiAddNextLevel = 100;

    private void LevelsChecking()
    {
        if (m_curExperessInLevels > m_requireExpressToNextLevel)
        {
            m_curExperessInLevels = 0;
            m_curlevel += 1;
            m_requireExpressToNextLevel = m_requireExpressToNextLevel + (m_curlevel - 1) * m_multiAddNextLevel;
        }
    }


    public float TimePlaying { get => m_timePlaying; }
    public Player Player { get => m_player; set => m_player = value; }
    public float Score { get => m_Score; set => m_Score = value; }
    public int CountKillEnemy { get => m_countKillEnemy; set => m_countKillEnemy = value; }
    public float Coin { get => m_coin; set => m_coin = value; }
    public float CurExpressInLevels { get => m_curExperessInLevels; set => m_curExperessInLevels = value; }
    public float RequireExpressToNextLevel { get => m_requireExpressToNextLevel; set => m_requireExpressToNextLevel = value; }

    public override void Awake()
    {
        MakeSingleton(false);

        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (m_player == null || m_player.IsDeadActor) return;

        LevelsChecking();

        m_timePlaying += Time.deltaTime;

        IsStatusWaveChecking();

        InitDataFirstTimeForGame();
    }

    private void InitDataFirstTimeForGame()
    {
        InvokeStartWave();
    }

    private void InvokeStartWave()
    {
        if (!m_isStaringWave)
        {
            m_isStaringWave = true;
            Debug.Log("Index-Wave: " + m_currentIndexWave);
            StartCoroutine(StartWaveCo());
        }
    }

    private IEnumerator StartWaveCo()
    {
        for (int i = 0; i < numberEnemyInWave; i++)
        {
            int indexNormalEnemyRandom = Random.Range(0, normalEnemyPrefabs.Count);

            Vector3 indexSpawnEnemyRandom = new Vector3(Random.Range(posXLeftPosSpawnEnemyLimit, posXRightSpawnEnemyLimit),
                                                        Random.Range(posYDownPosSpawnEnemyLimit, posYUpPosSpawnEnemyLimit), 0);

            GameObject gameObjectClone = Instantiate(normalEnemyPrefabs[indexNormalEnemyRandom].gameObject, indexSpawnEnemyRandom, Quaternion.identity);
            Actor enemyActor = gameObjectClone.GetComponent<Actor>();

            UpdateIndexForNormalAndBossEnemy(enemyActor);

            yield return new WaitForSeconds(timeSpawnBetweenEnemy);
        }


        if (m_currentIndexWave == indexWaveStartInitBoss)
        {
            for (int i = 0; i < numberBossInWave; i++)
            {
                int indexBossEnemyRandom = Random.Range(0, bossNormalEnemyPrefabs.Count);

                Vector3 indexSpawnBossRandom = new Vector3(Random.Range(posXLeftPosSpawnEnemyLimit, posXRightSpawnEnemyLimit),
                                                            Random.Range(posYDownPosSpawnEnemyLimit, posYUpPosSpawnEnemyLimit), 0);

                GameObject gameObjectClone = Instantiate(bossNormalEnemyPrefabs[indexBossEnemyRandom].gameObject, indexSpawnBossRandom, Quaternion.identity);
                Actor enemyActor = gameObjectClone.GetComponent<Actor>();
                UpdateIndexForNormalAndBossEnemy(enemyActor);
                yield return new WaitForSeconds(timeSpawnBetweenBoss);
            }

            indexWaveStartInitBoss += 1;
        }

        m_currentIndexWave += 1;
        UpdateIndexForWave(m_currentIndexWave);
    }

    private void IsStatusWaveChecking()
    {
        EnemyClose enemyClose = GameObject.FindObjectOfType<EnemyClose>();
        EnemyRemote enemyRemote = GameObject.FindObjectOfType<EnemyRemote>();

        if (enemyClose == null && enemyRemote == null)
        {
            m_isStaringWave = false;
        }
    }


    private void UpdateIndexForNormalAndBossEnemy(Actor enemyNormalActor)
    {
        Actor enemyActor = enemyNormalActor;

        if (enemyActor.GetType() == typeof(EnemyClose))
        {
            EnemyClose enemyClose = (EnemyClose)enemyActor;

            float newHP = enemyClose.CurrentHp + ((m_currentIndexWave - 1) * (multiToAddHp));
            enemyClose.CurrentHp = Mathf.Clamp(newHP, enemyClose.actorStat.Hp, 1000);

            float newDame = enemyClose.CurrentDamage + ((m_currentIndexWave - 1) * (multiToAddDame));
            enemyClose.CurrentDamage = Mathf.Clamp(newDame, enemyClose.CloseEnemyStat.Damage, 200);

            float newMoveSpeed = enemyClose.CurMoveSpeedStat + ((m_currentIndexWave - 1) * (multiToAddMoveSpeed));
            enemyClose.CurMoveSpeedStat = Mathf.Clamp(newMoveSpeed, enemyClose.CloseEnemyStat.MoveSpeed, 200);

            float newChassingSpeed = enemyClose.CurChassingStat + ((m_currentIndexWave - 1) * (multiToAddChassingSpeed));
            enemyClose.CurChassingStat = Mathf.Clamp(newChassingSpeed, enemyClose.CloseEnemyStat.ChassingSpeed, 200);

            float newAttackRate = enemyClose.CurAttackRate - ((m_currentIndexWave - 1) * (multiAddAttackRate));
            enemyClose.CurAttackRate = Mathf.Clamp(newAttackRate, 0.5f, 100);

            //Debug.Log("Name GameObject: " + enemyClose.name + " | " +
            //          "Hp: " + enemyClose.CurrentHp + " | " +
            //          "Damage: " + enemyClose.CurrentDamage + " | " +
            //          "Move Speed: " + enemyClose.CurrentMoveSpeed + " | " +
            //          "Move Speed Stat: " + enemyClose.CurMoveSpeedStat + " | " +
            //          "Chassing Speed Stat:" + enemyClose.CurChassingStat + " | " +
            //          "Attack Rate:" + enemyClose.CurAttackRate );
        }
        else if (enemyActor.GetType() == typeof(EnemyRemote))
        {
            EnemyRemote enemyRemote = (EnemyRemote)enemyActor;

            float newHP = enemyRemote.CurrentHp + ((m_currentIndexWave - 1) * (multiToAddHp));
            enemyRemote.CurrentHp = Mathf.Clamp(newHP, enemyRemote.actorStat.Hp, 1000);

            float newDame = enemyRemote.CurrentDamage + ((m_currentIndexWave - 1) * (multiToAddDame));
            enemyRemote.CurrentDamage = Mathf.Clamp(newDame, enemyRemote.RemoteEnemyStat.Damage, 200);

            float newMoveSpeed = enemyRemote.CurSpeedStat + ((m_currentIndexWave - 1) * (multiToAddMoveSpeed));
            enemyRemote.CurSpeedStat = Mathf.Clamp(newMoveSpeed, enemyRemote.RemoteEnemyStat.MoveSpeed, 200);

            float newChassingSpeed = enemyRemote.CurChassingStat + ((m_currentIndexWave - 1) * (multiToAddChassingSpeed));
            enemyRemote.CurChassingStat = Mathf.Clamp(newChassingSpeed, enemyRemote.RemoteEnemyStat.ChassingSpeed, 200);

            float newAttackRate = enemyRemote.CurAttackRate - ((m_currentIndexWave - 1) * (multiAddAttackRate));
            enemyRemote.CurAttackRate = Mathf.Clamp(newAttackRate, 1, 100);

            float newFlySpeedBullet = enemyRemote.CurFlySpeedBulletStat + ((m_currentIndexWave - 1) * (multiAddFlySpeedBullet));
            enemyRemote.CurFlySpeedBulletStat = Mathf.Clamp(newFlySpeedBullet, enemyRemote.RemoteEnemyStat.bulletFlySpeed, 100);


            //Debug.Log("Name GameObject: " + enemyRemote.name + " | " +
            //          "Hp: " + enemyRemote.CurrentHp + " | " +
            //          "Damage: " + enemyRemote.CurrentDamage + " | " +
            //          "Move Speed: " + enemyRemote.CurrentMoveSpeed + " | " +
            //          "Move Speed Stat: " + enemyRemote.CurSpeedStat + " | " +
            //          "Chassing Speed Stat:" + enemyRemote.CurChassingStat + " | " +
            //          "Attack Rate:" + enemyRemote.CurAttackRate + " | " +
            //          "Fly Bullet: " + enemyRemote.CurFlySpeedBulletStat);
        }
    }

    private void UpdateIndexForWave(int currentIndexWave)
    {
        int newNumberEnemyInWave = numberEnemyInWave + ((currentIndexWave - 1) * (multiNumberEnemyInWave));
        numberEnemyInWave = Mathf.Clamp(newNumberEnemyInWave, numberEnemyInWave, 1000);
        int newNumberBossInWave = numberBossInWave + ((currentIndexWave - 1) * (multiNumberBossInWave));
        numberBossInWave = Mathf.Clamp(newNumberBossInWave, numberBossInWave, 1000);
        float newTimeSpawnBetweenEnemy = timeSpawnBetweenEnemy - ((currentIndexWave - 1) * (multiTimeSpawnBetweenEnemy));
        timeSpawnBetweenEnemy = Mathf.Clamp(newTimeSpawnBetweenEnemy, 1, 100);
        float newTimeSpawnBetweenBoss = timeSpawnBetweenBoss - ((currentIndexWave - 1) * (multiNumberBossInWave));
        timeSpawnBetweenBoss = Mathf.Clamp(newTimeSpawnBetweenBoss, 1, 100);
    }

}
