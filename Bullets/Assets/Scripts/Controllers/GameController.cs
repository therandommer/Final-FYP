using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject spawnHolder;
    private void OnEnable()
    {
        Actions.OnPlayerKilled += playerDead;
    }
    private void OnDisable()
    {
        Actions.OnPlayerKilled -= playerDead;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
		{
            Actions.OnPause();
            Debug.Log("Pausing");
		}
    }
    private void playerDead(Player playerRef)
	{

	}
}
