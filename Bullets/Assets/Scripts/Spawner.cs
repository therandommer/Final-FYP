using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SpawnRequirements
{
    public float activateTimer;//what time it will spawn the object
    public GameObject spawnObject;//object reference
    public int spawnNumber;//number of objects to spawn
    public float spawnDelay;//delay between each entity spawning
    public bool usesThisPos = false;
    public Vector2 actualSpawnPoint;//if not using the default position of this spawn point
    public Enemy.StartDirection startDirection;
}
public class Spawner : MonoBehaviour
{
    public List<SpawnRequirements> spawnReq;
    int currentSpawn = 0; //index number
    TimeController timeC;
    bool isSpawning = false;
    GameObject spawnParent;
    void OnEnable()
	{
        spawnParent = GameObject.Find("SpawnParent");
    }
    void OnDisable()
	{

	}
    void Start()
    {
        timeC = FindObjectOfType<TimeController>();
        StartCoroutine(CheckTimer());
        spawnReq = spawnReq.OrderBy(w=>w.activateTimer).ToList(); //sort list by activation timer
    }

    IEnumerator CheckTimer()
	{
        
        yield return new WaitUntil(() => !timeC.isPaused); //holds coroutine until unpaused
	}
    
    IEnumerator SpawnObject()
	{
        if(!timeC.isPaused)
		{
            for(int i = 0; i<spawnReq[currentSpawn].spawnNumber; ++i)
			{
                if (spawnReq[currentSpawn].usesThisPos)
                {
                    GameObject spawnedObject = Instantiate(spawnReq[currentSpawn].spawnObject, gameObject.transform, false);
                    
                    if (spawnedObject.GetComponent<EnemyGameplay>())
					{
                        EnemyGameplay enemyScript = spawnedObject.GetComponent<EnemyGameplay>();
                        enemyScript.thisEnemy.thisDirection = spawnReq[currentSpawn].startDirection;
                        if (!spawnParent)
                        {
                            spawnParent = GameObject.Find("SpawnParent");
                        }
                        spawnedObject.transform.parent = spawnParent.transform;
                        //Debug.Log($"Created enemy has {enemyScript.thisEnemy.thisDirection} as a default direction");
                    }
                }
                else if (!spawnReq[currentSpawn].usesThisPos)
                {
                    Vector2 thisPosition = spawnReq[currentSpawn].actualSpawnPoint;
                    GameObject spawnedObject = Instantiate(spawnReq[currentSpawn].spawnObject, thisPosition, this.transform.rotation, gameObject.transform) as GameObject;
                    if (spawnedObject.GetComponent<EnemyGameplay>())
                    {
                        EnemyGameplay enemyScript = spawnedObject.GetComponent<EnemyGameplay>();
                        enemyScript.thisEnemy.thisDirection = spawnReq[currentSpawn].startDirection;
                        //Debug.Log($"Created enemy has {enemyScript.thisEnemy.thisDirection} as a default direction");
                    }
                    if (!spawnParent)
                    {
                        spawnParent = GameObject.Find("SpawnParent");
                    }
                    spawnedObject.transform.parent = spawnParent.transform;
                }
                yield return new WaitForSeconds(spawnReq[currentSpawn].spawnDelay);
            }
                currentSpawn++;
                isSpawning = false;
                StopCoroutine(SpawnObject());
        }
        /*if(timeC.isPaused)
		{
            yield return new WaitUntil(() => !timeC.isPaused); //holds coroutine until unpaused
        }*/
    }
    void Update()
    {
        if (!timeC.isPaused)
        {
            if(currentSpawn<spawnReq.Count)
			{
                if (timeC.timePassed >= spawnReq[currentSpawn].activateTimer && !isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(SpawnObject());
                }
                timeC.timeToNextSpawn = spawnReq[currentSpawn].activateTimer - timeC.timePassed;
            }
            if(currentSpawn == spawnReq.Count)
			{
                gameObject.SetActive(false);
			}
        } 
    }
}
