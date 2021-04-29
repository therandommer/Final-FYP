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
	public bool isSpawned = false;
	public Vector2 actualSpawnPoint;//if not using the default position of this spawn point
	public Enemy.StartDirection startDirection;
}
public class Spawner : MonoBehaviour
{
	public List<SpawnRequirements> spawnReq;
	int currentSpawn = 0; //index number
	TimeController timeC;
	GameObject spawnParent;
	public float speedScalar = 1; //sends this to the object it spawns
	void OnEnable()
	{
		spawnParent = GameObject.Find("SpawnParent");
		Actions.OnLevelRestart += Reset;
		Actions.OnLevelComplete += Stop;
		Actions.OnNewBPMSpeed += UpdateSpeedScalar;
	}
	void OnDisable()
	{
		Actions.OnLevelRestart -= Reset;
		Actions.OnLevelComplete -= Stop;
		Actions.OnNewBPMSpeed -= UpdateSpeedScalar;
	}
	void Start()
	{
		timeC = FindObjectOfType<TimeController>();
		spawnReq = spawnReq.OrderBy(w => w.activateTimer).ToList(); //sort list by activation timer
	}

	IEnumerator SpawnObject()
	{
		Debug.Log("Spawning number: " + (currentSpawn+1) + "/" + spawnReq.Count);
		for (int i = 0; i < spawnReq[currentSpawn].spawnNumber; ++i)
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
					spawnedObject.GetComponent<AIGameplay>().SendMessage("SetProjectileSpeedScalar", speedScalar);
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
				spawnedObject.GetComponent<AIGameplay>().SendMessage("SetProjectileSpeedScalar", speedScalar);
			}
			yield return new WaitForSeconds(spawnReq[currentSpawn].spawnDelay);
		}
		currentSpawn++;
		StopCoroutine(SpawnObject());
		/*if(timeC.isPaused)
		{
            yield return new WaitUntil(() => !timeC.isPaused); //holds coroutine until unpaused
        }*/
	}
	void Update()
	{
		if (currentSpawn+1 <= spawnReq.Count && spawnReq.Count > 0)
		{
			if (timeC.timePassed >= spawnReq[currentSpawn].activateTimer && spawnReq[currentSpawn].isSpawned == false)
			{
				spawnReq[currentSpawn].isSpawned = true;
				StartCoroutine(SpawnObject());
			}
			timeC.timeToNextSpawn = spawnReq[currentSpawn].activateTimer - timeC.timePassed;
		}
	}
	void UpdateSpeedScalar(int _index)
	{
		speedScalar = FindObjectOfType<GameController>().GetExistingIntensity(_index);
	}
	void Reset()
	{
		StopAllCoroutines();
		currentSpawn = 0;
		foreach (SpawnRequirements sR in spawnReq)
		{
			sR.isSpawned = false;
		}
		speedScalar = 1;
	}
	void Stop()
	{
		StopAllCoroutines();
	}
}
