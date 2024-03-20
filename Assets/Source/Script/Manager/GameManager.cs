using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Setting Wave: ")]
    public int numberEnemyInWave;
    public float timeSpawnBetweenEnemy;
    public int indexWaveStartInitBoss;
    public int numberBossInWave;
    public float timeSpawnBetweenBoss;

    [Header("Setting Multi Wave Index: ")]
    public float multiNumberEnemyInWave;
    public float multinumberBossInWave;
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
    public List<Actor> bossEnemyPrefabs;

    private Player m_player;

    private bool m_isStaringWave;
    private int m_currentIndexWave;

    public override void Awake()
    {
        MakeSingleton(false);

        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (m_player == null) return;

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
            ++m_currentIndexWave;
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

            if (enemyActor.GetType() == typeof(EnemyClose))
            {
                EnemyClose enemyClose = (EnemyClose)enemyActor;

                float newHP = enemyClose.CurrentHp + ((m_currentIndexWave - 1) * (multiToAddHp * 0.5f));
                enemyClose.CurrentHp = Mathf.Clamp(newHP, enemyClose.actorStat.Hp, 1000);

                float newDame = enemyClose.CurrentDamage + ((m_currentIndexWave - 1) * (multiToAddDame * 0.5f));
                enemyClose.CurrentDamage = Mathf.Clamp(newDame, enemyClose.CloseEnemyStat.Damage, 200);

                float newMoveSpeed = enemyClose.CurMoveSpeedStat + ((m_currentIndexWave - 1) * (multiToAddMoveSpeed * 0.5f));
                enemyClose.CurMoveSpeedStat = Mathf.Clamp(newMoveSpeed, enemyClose.CloseEnemyStat.MoveSpeed, 100f);

                float newChassingSpeed = enemyClose.CurChassingStat + ((m_currentIndexWave - 1) * (multiToAddChassingSpeed * 0.5f));
                enemyClose.CurChassingStat = Mathf.Clamp(newChassingSpeed, enemyClose.CloseEnemyStat.ChassingSpeed, 100f);

                float newAttackRate = enemyClose.CurAttackRate - ((m_currentIndexWave - 1) * (multiAddAttackRate * 0.5f));
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

                float newHP = enemyRemote.CurrentHp + ((m_currentIndexWave - 1) * (multiToAddHp * 0.5f));
                enemyRemote.CurrentHp = Mathf.Clamp(newHP, enemyRemote.actorStat.Hp, 1000);

                float newDame = enemyRemote.CurrentDamage + ((m_currentIndexWave - 1) * (multiToAddDame * 0.5f));
                enemyRemote.CurrentDamage = Mathf.Clamp(newDame, enemyRemote.RemoteEnemyStat.Damage, 200);

                float newMoveSpeed = enemyRemote.CurSpeedStat + ((m_currentIndexWave - 1) * (multiToAddMoveSpeed * 0.5f));
                enemyRemote.CurSpeedStat = Mathf.Clamp(newMoveSpeed, enemyRemote.RemoteEnemyStat.MoveSpeed, 100f);

                float newChassingSpeed = enemyRemote.CurChassingStat + ((m_currentIndexWave - 1) * (multiToAddChassingSpeed * 0.5f));
                enemyRemote.CurChassingStat = Mathf.Clamp(newChassingSpeed, enemyRemote.RemoteEnemyStat.ChassingSpeed, 100f);

                float newAttackRate = enemyRemote.CurAttackRate - ((m_currentIndexWave - 1) * (multiAddAttackRate * 0.5f));
                enemyRemote.CurAttackRate = Mathf.Clamp(newAttackRate, 1, 100);

                float newFlySpeedBullet = enemyRemote.CurFlySpeedBulletStat + ((m_currentIndexWave - 1) * (multiAddFlySpeedBullet * 0.5f));
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


            yield return new WaitForSeconds(timeSpawnBetweenEnemy);
        }


        if (m_currentIndexWave == indexWaveStartInitBoss)
        {
            for (int i = 0; i < numberBossInWave; i++)
            {
                int indexBossEnemyRandom = Random.Range(0, bossEnemyPrefabs.Count);

                Vector3 indexSpawnBossRandom = new Vector3(Random.Range(posXLeftPosSpawnEnemyLimit, posXRightSpawnEnemyLimit),
                                                            Random.Range(posYDownPosSpawnEnemyLimit, posYUpPosSpawnEnemyLimit), 0);

                GameObject gameObjectClone = Instantiate(bossEnemyPrefabs[indexBossEnemyRandom].gameObject, indexSpawnBossRandom, Quaternion.identity);

                yield return new WaitForSeconds(timeSpawnBetweenBoss);
            }

            indexWaveStartInitBoss += 1;
        }
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
}
