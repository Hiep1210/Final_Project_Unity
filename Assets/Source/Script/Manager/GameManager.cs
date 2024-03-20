using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Actor> normalEnemyPrefabs;
    public List<Actor> bossEnemyPrefabs;

    public AudioSource source;
    public AudioClip clip;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {
        source.clip = clip;
        source.loop = true;
        source.Play();
    }
}
