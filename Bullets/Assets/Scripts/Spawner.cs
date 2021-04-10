using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<float> activateTimer;
    public List<GameObject> spawnObject;
    public List<int> spawnNumber;
    TimeController timeC;
    void OnEnable()
	{

	}
    void OnDisable()
	{

	}
    void Start()
    {
        timeC = FindObjectOfType<TimeController>();
    }

    
    void Update()
    {
        
    }
}
