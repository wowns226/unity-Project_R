using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
    }

    [SerializeField]
    private List<GameObject> mobList;
    [SerializeField]
    private List<GameObject> spawnMobList;
    [SerializeField]
    private GameObject spawnBossPos;

    private int maxMobCount = 5;

    private float spawnDelay = 2f;

    private bool isInSpawner;

    public IReadOnlyList<GameObject> SpawnMobList => SpawnMobList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpawner = true;

            if(Player.instance.FieldIndex == 3 )
            {
                GameObject boss = mobList[3];
                Instantiate(boss,spawnBossPos.transform.position,spawnBossPos.transform.rotation);

            }
            else
            {
                StartCoroutine(StartSpawn(Player.instance.FieldIndex));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInSpawner = false;
            StopCoroutine(StartSpawn(Player.instance.FieldIndex));
            EndSpawn();
        }
    }

    private void CreateMob(GameObject mob)
    {
        var enemy = Instantiate<GameObject>(mob);
        spawnMobList.Add(enemy);

        enemy.GetComponent<NavMeshAgent>().enabled = false;
        enemy.transform.position = this.transform.position + new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        enemy.GetComponent<NavMeshAgent>().enabled = true;
    }

    private IEnumerator StartSpawn(int mobIndex)
    {
        while (isInSpawner)
        {
            int monsterCount = spawnMobList.Count;

            if( monsterCount == spawnMobList.Count )
            {
                yield return null;
            }

            if (monsterCount < maxMobCount)
            {
                yield return new WaitForSeconds(spawnDelay);

                if ( isInSpawner )
                    CreateMob(mobList[mobIndex]);
            }
        }
    }

    private void EndSpawn()
    {
        foreach(var mob in spawnMobList )
        {
            Destroy(mob);
        }

        spawnMobList.Clear();
    }

    public void DieToRemoveList( GameObject obj )
    {
        spawnMobList.Remove(obj);
    }
}
