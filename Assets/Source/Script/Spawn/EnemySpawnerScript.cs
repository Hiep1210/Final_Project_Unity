using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField]
    GameObject blueprint;
    [SerializeField]
    float spawnTime = 10;
    public GameObject spawnx;
    int poolsize = 10;
    List<GameObject> list;
    AutoMoveFree enemyScript;
    float count = 0;
    public SpriteRenderer spriteRenderer;
    private float screenwidth, screenheight;
    GameObject enemy = null;
    // Start is called before the first frame update
    void Start()
    {
        screenwidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenheight = Camera.main.orthographicSize;
        list = new List<GameObject>();
        for (int i = 0; i < poolsize; i++)
        {
            GameObject enemy = Instantiate(blueprint);
            enemy.SetActive(false);
            list.Add(enemy);
        }
    }

    void RandomMonsterSpawn()
    {
        //random prefab
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count > spawnTime)
        {
            StartSpawning();
            count = 0;
        }
    }
    void StartSpawning()
    {
        int number = Random.Range(1, 4);
        foreach (GameObject avai in list)
        {
            if (avai.activeSelf == false && number > 0)
            {
                StartCoroutine(Spawn(avai));
                number--;
            }
        }
    }

    IEnumerator Spawn(GameObject avai)
    {
        GameObject spawn = Instantiate(spawnx);
        spawn.transform.position = new Vector2(Random.Range(-screenwidth, screenwidth), Random.Range(-screenheight, screenheight));
        yield return new WaitForSeconds(3f);
        avai.SetActive(true);
        enemy = avai;
        enemyScript = enemy.GetComponent<AutoMoveFree>();
        enemy.transform.position = spawn.transform.position;
        GameObject.Destroy(spawn);
        enemyScript.MoveSpeed = Random.Range(5, 8);
    }

}
