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
	//used with random spawning
	public List<GameObject> potentialEnemies = new List<GameObject>();
	public List<GameObject> potentialEnemiesLate = new List<GameObject>();
	bool isLateSpawn = false;
	int thisSeed = 123456789;
	int spawnNumber = 1;
	public Enemy.StartDirection defaultStartDirection;
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
		thisSeed = FindObjectOfType<ModController>().GetSeed();
		Random.InitState(thisSeed);
	}
	IEnumerator SpawnEnemies(int neededSpawns) //spawn enemies every 0.4, repeat each time passed from GameSpawnController
	{
		int tmpSpawns = neededSpawns;
		
		if(!isLateSpawn)
		{
			int tmpI = Random.Range(0, potentialEnemies.Count);
			for (int i = 0; i < neededSpawns; ++i)
			{
				GameObject spawnedObject = Instantiate(potentialEnemies[tmpI], gameObject.transform, false);
				EnemyGameplay enemyScript = spawnedObject.GetComponent<EnemyGameplay>();
				enemyScript.thisEnemy.thisDirection = defaultStartDirection;
				if (!spawnParent)
				{
					spawnParent = GameObject.Find("SpawnParent");
				}
				spawnedObject.transform.parent = spawnParent.transform;
				spawnedObject.GetComponent<AIGameplay>().SendMessage("SetProjectileSpeedScalar", speedScalar);
				//Debug.Log("Waiting");
				yield return new WaitForSeconds(0.4f);
			}
		}
		else //gets tougher after 1/3 of the song
		{
			int tmpI = Random.Range(0, potentialEnemiesLate.Count);
			Debug.Log("Spawning tougher");
			for (int i = 0; i < neededSpawns; ++i)
			{
				GameObject spawnedObject = Instantiate(potentialEnemiesLate[tmpI], gameObject.transform, false);
				EnemyGameplay enemyScript = spawnedObject.GetComponent<EnemyGameplay>();
				enemyScript.thisEnemy.thisDirection = defaultStartDirection;
				if (!spawnParent)
				{
					spawnParent = GameObject.Find("SpawnParent");
				}
				spawnedObject.transform.parent = spawnParent.transform;
				spawnedObject.GetComponent<AIGameplay>().SendMessage("SetProjectileSpeedScalar", speedScalar);
				//Debug.Log("Waiting");
				yield return new WaitForSeconds(0.4f);
			}
		}
		StopCoroutine(SpawnEnemies(tmpSpawns));
	}
	IEnumerator SpawnObject()
	{
		//Debug.Log("Spawning number: " + (currentSpawn+1) + "/" + spawnReq.Count);
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
		///Old Spawning System
		/*if (currentSpawn+1 < spawnReq.Count && spawnReq.Count > 0)
		{
			if (timeC.timePassed >= spawnReq[currentSpawn].activateTimer && spawnReq[currentSpawn].isSpawned == false)
			{
				spawnReq[currentSpawn].isSpawned = true;
				StartCoroutine(SpawnObject());
			}
			timeC.timeToNextSpawn = spawnReq[currentSpawn].activateTimer - timeC.timePassed;
		}*/
	}
	void UpdateSpeedScalar(int _index)
	{
		if(_index < FindObjectOfType<GameController>().GetIntensityCount())
		{
			speedScalar = FindObjectOfType<GameController>().GetExistingIntensity(_index);
		}
		//Debug.LogError($"Index for speed scalar update out of range, received index: {_index}, max allowed: {FindObjectOfType<GameController>().GetIntensityCount()}");
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
	public void SpawnEnemy(int _numberSpawned)
	{
		if(timeC.timePassed < timeC.maxTime/3 && isLateSpawn)
		{
			isLateSpawn = false;
		}
		else if(timeC.timePassed>=timeC.maxTime/3 && !isLateSpawn)
		{
			isLateSpawn = true;
		}
		spawnNumber = _numberSpawned;
		StopCoroutine(SpawnEnemies(_numberSpawned)); //should prevent the spam
		StartCoroutine(SpawnEnemies(_numberSpawned));
	}
}
